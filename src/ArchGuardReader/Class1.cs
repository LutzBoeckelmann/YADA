using System.Text;
using YADA.ArchGuard.BuildingBlock.Impl;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ArchGuardReader
{
    public class BehaviorDTO : IBehaviorDTO
    {
        public List<string> Internal { get; set; }
        public List<string> Container { get; set; }
    }

    public class YamlBehaviorDTO
    {
        [YamlMember(Alias = "internal")]
        public List<string> Internal { get; set; }
        [YamlMember(Alias = "container")]
        public List<string> Container { get; set; }

        public IBehaviorDTO Create()
        {
            return new BehaviorDTO { Internal = Internal, Container = Container };
        }
    }

    public class BuildingBlockDTO : IBuildingBlockDTO
    {
        public string Name { get; set; }
        public List<string> Filter { get; set; }
        public IBehaviorDTO Behavior { get; set; }
        public List<IBuildingBlockDTO> Children { get; set; }
    }

    public class YamlBuildingBlockDTO
    {
        public string name { get; set; }
        public List<string> filter { get; set; }
        public YamlBehaviorDTO behavior { get; set; }
        public List<YamlBuildingBlockDTO> children { get; set; }

        public IBuildingBlockDTO Create()
        {
            var childrenList = children == null ? null : children.Select(c => c.Create()).ToList();

            return new BuildingBlockDTO { Name = name, Filter = filter, Behavior = behavior.Create(), Children = childrenList };
        }
    }

    public class YamlProject
    {
        private YamlBuildingBlockDTO Map(IBuildingBlockDTO root)
        {
            return InternalMap(root);
        }
        private YamlBuildingBlockDTO InternalMap(IBuildingBlockDTO block)
        {
            var result = new YamlBuildingBlockDTO();

            result.name = block.Name;
            result.filter = block.Filter;

            result.behavior = new YamlBehaviorDTO() { Container = block.Behavior.Container, Internal = block.Behavior.Internal };
            if (block.Children != null)
            {
                result.children = new List<YamlBuildingBlockDTO>();
                foreach (var child in block.Children)
                {
                    result.children.Add(InternalMap(child));
                }
            }
            return result;
        }

        public IBuildingBlockDTO Get(BuildingBlock root)
        {
            return InternalGet(root);
        }

        private IBuildingBlockDTO InternalGet(BuildingBlock block)
        {
            var result = new BuildingBlockDTO();
            result.Name = block.Description.Name;
            result.Filter = block.Description.TypeFilter.AsString.ToList();
            result.Behavior = new BehaviorDTO() { Container = block.Behavior.ContainerBehavior.AsString, Internal = block.Behavior.InternalBehavior.AsString };
            if (block.Children != null && block.Children.Count > 0)
            {
                result.Children = new List<IBuildingBlockDTO>();
                foreach (var child in block.Children)
                {
                    result.Children.Add(InternalGet((BuildingBlock)child));
                }
            }
            return result;
        }

        public YamlBuildingBlockDTO ReadProject(string file)
        {

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .Build();

            var yaml = File.ReadAllText(file);

            try
            {
                return deserializer.Deserialize<YamlBuildingBlockDTO>(yaml);
            }
            catch (YamlException e)
            {
                var i = e.Message;
            }

            return null;
        }

        public YamlBuildingBlockDTO DeserializeProject(string project)
        {
            var deserializer = new DeserializerBuilder()
              .WithNamingConvention(UnderscoredNamingConvention.Instance)
              .Build();

            try
            {
                return deserializer.Deserialize<YamlBuildingBlockDTO>(project);
            }
            catch (YamlException e)
            {
                var i = e.Message;
            }

            return null;
        }

        public void WriteProject(IBuildingBlockDTO buildingBlockDTO, string file)
        {
            var yamlDto = Map(buildingBlockDTO);

            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var yaml = serializer.Serialize(yamlDto);
            File.WriteAllText(file, yaml);
        }

        public string SerializeProject(IBuildingBlockDTO buildingBlockDTO)
        {
            var yamlDto = Map(buildingBlockDTO);

            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var yaml = serializer.Serialize(yamlDto);
            StringBuilder sb = new StringBuilder();
            sb.Append(yaml);
            return sb.ToString();
        }
    }
}
// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System.Collections.Generic;
using YADA.DependencyRuleEngine.Feedback.Recorder.Recordings;

namespace YADA.DependencyRuleEngine.Feedback.Recorder
{
    internal static class FeedbackPrinter 
    {
        public static IList<string> Print(List<TypeRecording> types) 
        {
            IList<string> result = new List<string>();
            
            foreach(TypeRecording recordedType in types) 
            {
                result.Add($"Type: {recordedType.TypeName}");
            
                foreach(var rule in recordedType.Rules) 
                {
                    result.Add($"Rule: {rule.RuleName}");
                    
                    foreach(var dependency in rule.Dependencies) 
                    {
                        result.Add($"Dependency: {dependency.DependencyName}");
                        
                        foreach(var context in dependency.Contexts) 
                        {
                            result.Add($"Context: {context.Context}");
                        }
                    }

                    foreach(var info in rule.Infos) 
                    {
                        result.Add($"Info: {info.Info}");
                    }
                }
            }
            
            return result;
        }
    }
}
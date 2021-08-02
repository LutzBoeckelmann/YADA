// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

namespace YADA.Core.Analyser
{
    /// <summary>
    /// A context of an occurrence of a dependency. Supports the visitor pattern to discriminate 
    /// between different situations where a dependency may be found.
    /// </summary>
    public interface IDependencyContext 
    {
        /// <summary>
        /// A none generic visit method. Call the appropriate method at the visitor.
        /// </summary>
        /// <param name="visitor">The concrete visitor</param>
        void Visit(IDependencyContextVisitor visitor);
        /// <summary>
        /// A generic visit method. Call the appropriate method at the visitor.
        /// Enables the visitor to return generic typed values to make it more 
        /// handy to use.
        /// </summary>
        /// <param name="visitor">The visitor</param>
        /// <typeparam name="T">Generic type of the visitor</typeparam>
        /// <returns></returns>
        T Visit<T>(IDependencyContextVisitor<T> visitor);
    }
}

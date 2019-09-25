﻿using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA0001:Xml comment analysis disabled", Justification = "Not needed for tests projects")]
[assembly: SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Not needed for tests projects")]
[assembly: SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1652:Enable XML documentation output", Justification = "Not needed for tests projects")]
[assembly: SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1615:Element return value must be documented", Justification = "Not needed for tests projects")]
[assembly: SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1611:Element parameters must be documented", Justification = "Not needed for tests projects")]
[assembly: SuppressMessage("Microsoft.Design", "CA1063:Implement IDisposable correctly", Justification = "Not needed for tests projects")]
[assembly: SuppressMessage("Microsoft.Usage", "CA1816:Call GC.SuppressFinalize correctly", Justification = "Not needed for tests projects")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Conflicts with some standard patterns.")]
[assembly: SuppressMessage("Reliability", "CA2007:Do not directly await a Task", Justification = "Not needed for tests projects")]
[assembly: SuppressMessage("Usage", "CA2234:Pass system uri objects instead of strings", Justification = "String is fine - Uri class is sometime cumbersome.")]

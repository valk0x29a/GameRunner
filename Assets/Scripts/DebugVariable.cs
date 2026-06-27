using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Reflection;

[AttributeUsage(AttributeTargets.Field)]
public class DebugVariable : Attribute
{
    public string displayName;
    public DebugVariable(string name) { displayName = name; }
    public DebugVariable() { displayName = "~"; }
}


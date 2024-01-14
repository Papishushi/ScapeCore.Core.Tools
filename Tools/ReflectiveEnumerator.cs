/*
 * -*- encoding: utf-8 with BOM -*-
 * .▄▄ ·  ▄▄·  ▄▄▄·  ▄▄▄·▄▄▄ .     ▄▄·       ▄▄▄  ▄▄▄ .
 * ▐█ ▀. ▐█ ▌▪▐█ ▀█ ▐█ ▄█▀▄.▀·    ▐█ ▌▪▪     ▀▄ █·▀▄.▀·
 * ▄▀▀▀█▄██ ▄▄▄█▀▀█  ██▀·▐▀▀▪▄    ██ ▄▄ ▄█▀▄ ▐▀▀▄ ▐▀▀▪▄
 * ▐█▄▪▐█▐███▌▐█ ▪▐▌▐█▪·•▐█▄▄▌    ▐███▌▐█▌.▐▌▐█•█▌▐█▄▄▌
 *  ▀▀▀▀ ·▀▀▀  ▀  ▀ .▀    ▀▀▀     ·▀▀▀  ▀█▄▀▪.▀  ▀ ▀▀▀ 
 * https://github.com/Papishushi/ScapeCore
 * 
 * Copyright (c) 2023 Daniel Molinero Lucas
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.txt', which is part of this source code package.
 * 
 * ReflectiveEnumerator.cs
 * ReflectiveEnumerator provides methods for obtaining an enumerable of types that are subclasses of a specified type.
 */

using System.Reflection;

namespace ScapeCore.Core.Batching.Tools
{
    public static class ReflectiveEnumerator
    {
        public static IEnumerable<Type> GetEnumerableOfType<T>() => GetEnumerableOfType(typeof(T));
        public static IEnumerable<Type> GetEnumerableOfType(Type type)
        {
            List<Type> types = new();
            var assembly = Assembly.GetAssembly(type);
            if (assembly == null)
                return Enumerable.Empty<Type>();
            foreach (var subclassType in assembly.GetTypes().Where(x => x.IsSubclassOf(type)))
                types.Add(subclassType);
            return types;
        }
    }
}
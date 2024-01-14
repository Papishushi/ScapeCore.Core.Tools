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
 * DeeplyMutableType.cs
 * Provides support for deeply mutable types in a more type-safe environment.
 * This class substitute the use of object or dynamic in multiple source code
 * files from the core. This class works in conjunction to DeeplyMutable<T>,
 * which implements by inheritance a generic solution to the deeply mutable 
 * types supporting easy conversions.
 */

using System.Dynamic;
using System.Reflection;

namespace ScapeCore.Core.Batching.Tools
{
    public class DeeplyMutableType : DynamicObject
    {
        protected const string ERROR_FORMAT = "There was an error while setting a member from a deeply mutable type.";
        protected dynamic? _value;
        public dynamic? Value { get => _value ??= default; set => _value = value; }
        public DeeplyMutableType() { _value = default; }
        public DeeplyMutableType(object? value) => _value = value;

        protected virtual FieldInfo[]? DynamicFields => _value?.GetType().GetFields();
        protected static bool IsValueMember(string name) => name.Equals(nameof(Value), StringComparison.OrdinalIgnoreCase) || name.Equals(nameof(_value), StringComparison.OrdinalIgnoreCase);

        protected bool TryFieldOperation(string name, Func<FieldInfo, object?, bool> operation, object? result) =>
            DynamicFields?.FirstOrDefault(field => name.Equals(field.Name, StringComparison.OrdinalIgnoreCase)) switch
            {
                { } field => operation(field, result),
                _ => false
            };

        protected bool TryGetValueFromField(FieldInfo field, object? result)
        {
            try
            {
                result = field.GetValue(_value);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ERROR_FORMAT, ex);
            }
        }
        protected bool TrySetValueToField(FieldInfo field, object? value)
        {
            try
            {
                field.SetValue(_value, value);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ERROR_FORMAT, ex);
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object? result)
        {
            result = null;
            if (IsValueMember(binder.Name))
            {
                result = _value;
                return true;
            }
            return TryFieldOperation(binder.Name, TryGetValueFromField, result);
        }
        public override bool TrySetMember(SetMemberBinder binder, object? value)
        {
            if (IsValueMember(binder.Name))
            {
                _value = value;
                return true;
            }
            return TryFieldOperation(binder.Name, TrySetValueToField, value);
        }
    }
}

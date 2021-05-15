// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Components;

namespace ToreAurstadIT.BlazorEnumSelect.ClassLibrary
{
    /// <summary>
    /// Describes context for an <see cref="InputRadio{TValue}"/> component.
    /// </summary>
    public class CustomInputRadioContext
    {
        private readonly CustomInputRadioContext? _parentContext;

        /// <summary>
        /// Gets the name of the input radio group.
        /// </summary>
        public string GroupName { get; }

        /// <summary>
        /// Gets the current selected value in the input radio group.
        /// </summary>
        public object? CurrentValue { get; }

        /// <summary>
        /// Gets a css class indicating the validation state of input radio elements.
        /// </summary>
        public string FieldClass { get; }

        /// <summary>
        /// Gets the event callback to be invoked when the selected value is changed.
        /// </summary>
        public EventCallback<ChangeEventArgs> ChangeEventCallback { get; }

        /// <summary>
        /// Instantiates a new <see cref="InputRadioContext" />.
        /// </summary>
        /// <param name="parentContext">The parent <see cref="InputRadioContext" />.</param>
        /// <param name="groupName">The name of the input radio group.</param>
        /// <param name="currentValue">The current selected value in the input radio group.</param>
        /// <param name="fieldClass">The css class indicating the validation state of input radio elements.</param>
        /// <param name="changeEventCallback">The event callback to be invoked when the selected value is changed.</param>
        public CustomInputRadioContext(
            CustomInputRadioContext? parentContext,
            string groupName,
            object? currentValue,
            string fieldClass,
            EventCallback<ChangeEventArgs> changeEventCallback)
        {
            _parentContext = parentContext;

            GroupName = groupName;
            CurrentValue = currentValue;
            FieldClass = fieldClass;
            ChangeEventCallback = changeEventCallback;
        }

        /// <summary>
        /// Finds an <see cref="InputRadioContext"/> in the context's ancestors with the matching <paramref name="groupName"/>.
        /// </summary>
        /// <param name="groupName">The group name of the ancestor <see cref="InputRadioContext"/>.</param>
        /// <returns>The <see cref="InputRadioContext"/>, or <c>null</c> if none was found.</returns>
        public CustomInputRadioContext? FindContextInAncestors(string groupName)
            => string.Equals(GroupName, groupName) ? this : _parentContext?.FindContextInAncestors(groupName);
    }
}
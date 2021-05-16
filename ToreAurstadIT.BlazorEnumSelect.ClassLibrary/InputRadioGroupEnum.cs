// file: Shared/InputSelectEnum.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Humanizer;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace ToreAurstadIT.BlazorEnumSelect.ClassLibrary
{
    /// <summary>
    /// Groups child input radio buttons with tailoring for enum type. Supports data binding and resource files.
    /// The group can be stacked either horizontally or vertically using the StackMode property on this control
    /// Enum input radio button group control which supports generation of options from enum type of data bound property. 
    /// With ideas from: <see href="https://www.meziantou.net/creating-a-inputselect-component-for-enumerations-in-blazor.htm" />
    /// with some additional features. This sample also supports nullable enumerable enum type. <br />
    /// Class is now not sealed in case you want to adapt the enum control more. <br />
    /// <ul>
    ///  <li>The numeric value is sorted ascending numerically</li> <br />
    ///  <li>Parameter StackMode controls whether the controls stacks input radio buttons horizontally or vertically.</li>
    ///  <li>Parameter AdditionalCssClasses can be set to a string to customize the css class(es) of the input type radio elements. E.g. Blazorise uses "custom-control-input" for radio buttons and same for the wrapping div element. It is possible to add multiple CSS classes here which will be added to those that InputSelect base class also adds.</li>
    ///  <li>Parameter AdditionalCssClassesLabel sets the css class of the label. E.g. Blazorise uses "custom-control-label".</li>
    /// <li>Paramter AdditionalCssClassesDiv sets the css class of the div. E.g. Blazorise uses "custom-control custom-radio custom-control-inline"</li>
    ///  Parameter ShowIntValues to show enum value also in the text of option which defaults to true <br />
    /// Parameter EmptyTextValue will if set to non null value will check if the int value is equal to this set empty text for the option element <br />
    /// </ul>
    /// Inherit from InputBase so the hard work is already implemented 
    /// // Note that adding a constraint on TEnum (where T : Enum) doesn't work when used in the view, Razor raises an error at build time. Also, this would prevent using nullable types...
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    public class InputRadioGroupEnum<TEnum> : InputRadioGroup<TEnum>
    {
       
        private CustomInputRadioContext _context;

        private readonly string _defaultGroupName = Guid.NewGuid().ToString("N");

        [CascadingParameter] public CustomInputRadioContext? CascadedContextParent { get; set; }

        [Parameter]
        public bool ShowIntValues { get; set; } = true;

        [Parameter]
        public int? EmptyTextValue { get; set; }

        [Parameter]
        public string AdditionalCssClasses { get; set; }

        [Parameter]
        public string AdditionalCssClassesLabel { get; set; }

        [Parameter]
        public string AdditionalCssClassesDiv { get; set; }

        [Parameter]
        public StackMode StackMode { get; set; }

        private List<object> EnumValuesSortedNumerically = new List<object>();

        protected override void OnParametersSet()
        {
            var groupName = !string.IsNullOrEmpty(Name) ? Name : _defaultGroupName;
            var fieldClass = EditContext.FieldCssClass(FieldIdentifier);
            var changeEventCallback = EventCallback.Factory.CreateBinder<string?>(this, __value => CurrentValueAsString = __value, CurrentValueAsString);
            _context = new CustomInputRadioContext(CascadedContextParent, groupName, CurrentValue, fieldClass, changeEventCallback);
        }

        // Generate html when the component is rendered.
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var enumType = GetEnumType();
            BuildEnumValuesSorted(enumType);

            //builder.OpenComponent<CascadingValue<CustomInputRadioContext>>(0);
            //builder.SetKey(_context);
            //builder.AddAttribute(1, "IsFixed", true);
            //builder.AddAttribute(2, "Value", _context);
            //builder.AddAttribute(3, "Name", _defaultGroupName);

            //builder.AddAttribute(3, "ChildContent", ChildContent);

            //builder.OpenComponent<InputRadioGroup<TEnum>>(0);
            ////builder.AddAttribute(1, "IsFixed", true);
            //builder.AddAttribute(1, "Name", _defaultGroupName);
            //Console.WriteLine("Value expression: " + this.ValueExpression);

            //builder.AddAttribute(2, "ValueExpression", this.ValueExpression);

            string compoundCssClassDiv = !string.IsNullOrWhiteSpace(AdditionalCssClassesDiv) ? $"{AdditionalCssClassesDiv} {CssClass}" : CssClass;
    
            builder.OpenElement(0, "div");

            builder.AddContent(2, new RenderFragment((childBuilder) =>
            { 
                //builder.OpenComponent<InputRadioGroup<TEnum>>(0);
                //builder.AddAttribute(1, "IsFixed", true);
                //builder.AddAttribute(2, "Value", _context);
                //builder.AddAttribute(3, "Name", _defaultGroupName);

                string compoundCssClass = !string.IsNullOrWhiteSpace(AdditionalCssClasses) ? $"{AdditionalCssClasses} {CssClass}" : CssClass;

                string compoundCssClassLabel = !string.IsNullOrWhiteSpace(AdditionalCssClassesLabel) ? $"{AdditionalCssClassesLabel} {CssClass}" : CssClass;
                
                //TODO: add left and right arrow navigation

                builder.OpenElement(0, "script");
                builder.AddContent(0, @"
                function checkKey(e) {

                    e = e || window.event;
                    console.log(e);
                    if (e.keyCode == '37') {
                         console.log('left arrow');
                    }
                    if (e.keyCode == '39') {
                        console.log('right arrow'); 
                    }
                }   
                ");
                builder.CloseElement();


                foreach (var value in EnumValuesSortedNumerically)
                {
                    string enumValueSuffix = this.ShowIntValues ? $"{Convert.ToInt32(value)}" : string.Empty;

                    childBuilder.OpenElement(0, "div");
                    childBuilder.AddAttribute(0, "class", compoundCssClassDiv);
                    childBuilder.AddAttribute(1, "onclick", "var $firstRadio = $(this).children('input:first'); if (!!$firstRadio) { $firstRadio.prop('checked', !$firstRadio.prop('checked')); }");

                    childBuilder.OpenElement(0, "input");
                    childBuilder.AddAttribute(1, "type", "radio");
                    childBuilder.AddAttribute(2, "name", _defaultGroupName);
                    childBuilder.AddAttribute(3, "value", value);
                    childBuilder.AddAttribute(4, "onchange", EventCallback.Factory.CreateBinder<string>(this, value => CurrentValueAsString = value, CurrentValueAsString, null));
                    childBuilder.AddAttribute(5, "class", compoundCssClass);
                    string newChildId = Guid.NewGuid().ToString("N"); 
                    childBuilder.AddAttribute(5, "id", newChildId);

                    //childBuilder.OpenComponent<InputRadio<TEnum>>(0);
                    ////childBuilder.SetKey(_context);
                    //childBuilder.AddAttribute(1, "Value", value);
                    ////childBuilder.AddAttribute(2, "Name", _context.GroupName);
                    //childBuilder.CloseComponent();

                    childBuilder.CloseElement();

                    childBuilder.OpenElement(0, "label");
                    childBuilder.AddAttribute(0, "for", value);
                    childBuilder.AddAttribute(1, "class", compoundCssClassLabel);
                    childBuilder.AddContent(0, $"{GetDisplayName((TEnum)value)} ({enumValueSuffix})");
                    childBuilder.CloseElement();

                    childBuilder.CloseElement(); //div

                    if (this.StackMode == StackMode.Vertical)
                    {
                        //simple vertical stacking - just add br tags
                        childBuilder.OpenElement(0, "br");
                        childBuilder.CloseElement();
                    }

                }


            }));

            builder.CloseElement();


            //builder.OpenElement(0, "select");
            //builder.AddMultipleAttributes(1, AdditionalAttributes);
            //string compoundCssClass = !string.IsNullOrWhiteSpace(AdditionalCssClasses) ? $"{AdditionalCssClasses} {CssClass}" : CssClass;
            //builder.AddAttribute(2, "class", compoundCssClass);
            //builder.AddAttribute(3, "value", BindConverter.FormatValue(CurrentValueAsString));
            //builder.AddAttribute(4, "onchange", EventCallback.Factory.CreateBinder<string>(this, value => CurrentValueAsString = value, CurrentValueAsString, null));

            //// Add an option element per enum value
            //var enumType = GetEnumType();
            //BuildEnumValuesSorted(enumType);

            //foreach (TEnum value in EnumValuesSortedNumerically)
            //{
            //    builder.OpenElement(5, "option");
            //    builder.AddAttribute(6, "value", value.ToString());

            //    string enumValuePrefix = this.ShowIntValues ? $"{Convert.ToInt32(value)} : " : string.Empty;

            //    if (EmptyTextValue.HasValue && Convert.ToInt32(value) == EmptyTextValue.Value)
            //    {
            //        builder.AddContent(7, string.Empty);
            //    }
            //    else
            //    {
            //        builder.AddContent(7, enumValuePrefix + GetDisplayName(value));
            //    }
            //    builder.CloseElement();
            //}

            //builder.CloseElement(); // close the select element
        }

        protected override bool TryParseValueFromString(string value, out TEnum result, out string validationErrorMessage)
        {
            // Let's Blazor convert the value for us 😊
            if (BindConverter.TryConvertTo(value, CultureInfo.CurrentCulture, out TEnum parsedValue))
            {
                result = parsedValue;
                validationErrorMessage = null;
                return true;
            }

            // Map null/empty value to null if the bound object is nullable
            if (string.IsNullOrEmpty(value))
            {
                var nullableType = Nullable.GetUnderlyingType(typeof(TEnum));
                if (nullableType != null)
                {
                    result = default;
                    validationErrorMessage = null;
                    return true;
                }
            }

            // The value is invalid => set the error message
            result = default;
            validationErrorMessage = $"The {FieldIdentifier.FieldName} field is not valid.";
            return false;
        }

        private void BuildEnumValuesSorted(Type t)
        {
            List<object> temp = new List<object>();
            foreach (var enumValue in t.GetEnumValues())
            {
                temp.Add(enumValue);
            }
            EnumValuesSortedNumerically = temp.OrderBy(t => (int)t).ToList();
        }

        // Get the display text for an enum value:
        // - Use the DisplayAttribute if set on the enum member, so this support localization
        // - Fallback on Humanizer to decamelize the enum member name
        private string GetDisplayName(TEnum value)
        {
            // Read the Display attribute name
            var member = value.GetType().GetMember(value.ToString())[0];
            var displayAttribute = member.GetCustomAttribute<DisplayAttribute>();
            if (displayAttribute != null)
                return displayAttribute.GetName();

            // Require the NuGet package Humanizer.Core
            // <PackageReference Include = "Humanizer.Core" Version = "2.8.26" />
            return value.ToString().Humanize();
        }

        // Get the actual enum type. It unwrap Nullable<T> if needed
        // MyEnum  => MyEnum
        // MyEnum? => MyEnum
        private Type GetEnumType()
        {
            var nullableType = Nullable.GetUnderlyingType(typeof(TEnum));
            if (nullableType != null)
                return nullableType;

            return typeof(TEnum);
        }
    }


}
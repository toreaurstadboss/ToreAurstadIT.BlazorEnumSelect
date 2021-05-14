
using System.ComponentModel.DataAnnotations;
using ToreAurstadIT.BlazorEnumSelect.SampleClient.Resources;

namespace ToreAurstadIT.BlazorEnumSelect.SampleClient.Models
{

    public enum SomeEnum
    {

        None = -1,

        [Display(Name = "SomeTestValueA display text")]
        SomeTestValueA = 1,

        SomeTestValueB = 2,

        [Display(Name = "SomeTestValueC display text")]
        SomeTestValueC = 3,

        SomeTestValueD = 4,

        SomeTestValueE = 6,

        [Display(Name = "SomeTestValueF", ResourceType = typeof(SomeResources))]
        SomeTestValueF = 7,

        [Display(Name = "SomeTestValueG", ResourceType = typeof(SomeResources))]
        SomeTestValueG = 8

    }

}

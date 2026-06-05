namespace JoeCardApplication.Models;
// Home Index.razor.........you can do it on these, I saw on a IAmTimCorey video
// I think you can use DI in .cshtml files too, research that and try in DependencyInjectionDemo.cshtml

// builder.Services.AddTransient<DependencyInjectionDemo>
        // .....in Program.cs, this adds it to our Services list, which means can ask for it whenever we need a dependency instead of instatiating a new one

public class DependencyInjectionDemo
{
    public int Value1 { get; set; }

    public int Value2 { get; set; }

    public DependencyInjectionDemo()
    {
        Value1 = Random.Shared.Next(minValue: 1, maxValue: 1001);
        Value2 = Random.Shared.Next(minValue: 1, maxValue: 1001);
    }
}

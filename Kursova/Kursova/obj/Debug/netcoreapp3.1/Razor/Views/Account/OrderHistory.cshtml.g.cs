#pragma checksum "D:\Visual Studio Projects\Kursova\Kursova\Views\Account\OrderHistory.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "259b1a82ae29a8449f53bd59407d83a3f46f58ac"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Account_OrderHistory), @"mvc.1.0.view", @"/Views/Account/OrderHistory.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\Visual Studio Projects\Kursova\Kursova\Views\_ViewImports.cshtml"
using Kursova;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Visual Studio Projects\Kursova\Kursova\Views\_ViewImports.cshtml"
using Kursova.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"259b1a82ae29a8449f53bd59407d83a3f46f58ac", @"/Views/Account/OrderHistory.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0187e2e936986cc3604ad942c816f39f7c49795b", @"/Views/_ViewImports.cshtml")]
    public class Views_Account_OrderHistory : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Kursova.ViewModels.OrderHistoryViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "D:\Visual Studio Projects\Kursova\Kursova\Views\Account\OrderHistory.cshtml"
  
    ViewData["Title"] = "OrderHistory";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>OrderHistory</h1>\r\n");
#nullable restore
#line 7 "D:\Visual Studio Projects\Kursova\Kursova\Views\Account\OrderHistory.cshtml"
 foreach (var order in Model.myOrders)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <h1>Order Date - ");
#nullable restore
#line 9 "D:\Visual Studio Projects\Kursova\Kursova\Views\Account\OrderHistory.cshtml"
                Write(order.Date);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h1>\r\n    <h1>Order Price - ");
#nullable restore
#line 10 "D:\Visual Studio Projects\Kursova\Kursova\Views\Account\OrderHistory.cshtml"
                 Write(order.TotalPrice);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h1>\r\n    <h1>Order Address - ");
#nullable restore
#line 11 "D:\Visual Studio Projects\Kursova\Kursova\Views\Account\OrderHistory.cshtml"
                   Write(order.DeliveryAddress);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h1>\r\n    <h1>---------------------------------------------</h1>\r\n");
#nullable restore
#line 13 "D:\Visual Studio Projects\Kursova\Kursova\Views\Account\OrderHistory.cshtml"
}

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Kursova.ViewModels.OrderHistoryViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591

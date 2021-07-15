<%@ Page Title="Access SQL" Language="C#" AutoEventWireup="true" CodeBehind="SqlAccess.aspx.cs" MasterPageFile="~/Site.Master" Inherits="BadWebApp.SqlAccess" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>This will consume Azure SQL.</h3>
    <br />
    <p>Enter the optional category filter (e.g: bike). Having ";" or "'" in the filter causes an error</p>
    <br />
    <span style="text-align: center; margin: auto; display: table">
    <asp:Label runat="server">Category Filter: </asp:Label>
    &nbsp;&nbsp;
    <asp:TextBox runat="server" ID="Filter" Text=""></asp:TextBox>
    &nbsp;<asp:Button runat="server" ID="Button1" Text="Update" BackColor="#66CCFF" Font-Bold="True" ForeColor="White" OnClick="Button1_Click" />
        </span>
    <br />
    <asp:Label runat="server" BackColor="Red" ForeColor="White" Font-Bold="true" ID="ErrorLabel" Text=""></asp:Label>
<asp:GridView ID="GridView1" runat="server" HorizontalAlign="Center" style="margin: 8px;">
    <RowStyle BackColor="White" Font-Italic="True" />
    <EmptyDataRowStyle BackColor="Yellow" />
    <PagerStyle BackColor="#FFC0C0" Font-Italic="True" Font-Underline="True" />                
    <SelectedRowStyle BackColor="#00C000" Font-Bold="True" Font-Italic="True" />
    <EditRowStyle BackColor="#0000C0" />
    <AlternatingRowStyle BackColor="Wheat" BorderColor="Green" BorderStyle="Dashed"   BorderWidth="1px" Font-Bold="True" />
    <FooterStyle BackColor="#00C0C0" />
    <HeaderStyle BackColor="PaleTurquoise" Font-Bold="True" Font-Italic="True" Font-Underline="True" />  
</asp:GridView>
<asp:ObjectDataSource ID="FunctionDataSource" runat="server"></asp:ObjectDataSource>
    
</asp:Content>
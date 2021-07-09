<%@ Page Title="Break Three" Language="C#" AutoEventWireup="true"  MasterPageFile="~/Site.Master" CodeBehind="BreakThree.aspx.cs" Inherits="BadWebApp.BreakThree" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>This will break things and you will never see this code.</h3>
    <p>Use this area to provide additional information.</p>
    <asp:Label runat="server">Count Limit: </asp:Label>
    <asp:TextBox runat="server" ID="LimitBox" Text=""></asp:TextBox>    
    <asp:TextBox runat="server" ID="ResultBox" Text=""></asp:TextBox> 
    <asp:Button ID="DoAction" Text="Do the calculation" runat="server" OnClick="DoAction_Click" />
</asp:Content>
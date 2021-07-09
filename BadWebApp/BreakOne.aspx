<%@ Page Title="Break One" Language="C#" AutoEventWireup="true"  MasterPageFile="~/Site.Master" CodeBehind="BreakOne.aspx.cs" Inherits="BadWebApp.BreakOne" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>This will break things.</h3>
    <p>Use this area to provide additional information.</p>
    <asp:Label runat="server">Value of Session: </asp:Label>
    <asp:TextBox runat="server" ID="SessionBox" Text=""></asp:TextBox>

</asp:Content>

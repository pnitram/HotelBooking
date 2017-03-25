<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication1._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Reserver ditt hotellrom idag!</h1>
            </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Bookinginformasjon</h2>
           <div>
                <asp:Label ID="Label1" runat="server" Text="Fornavn:"></asp:Label>

                &nbsp;&nbsp;<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            </div>
            <br/>
            <div>
                <asp:Label ID="Label2" runat="server" Text="Etternavn:"></asp:Label>

                <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
            </div>
            <br/><asp:Button ID="Button1" runat="server" Text="Registrer" CssClass="btn" Width="78px" OnClick="Button1_Click" />
            <br />
            <br />
            <asp:Label ID="Label3" runat="server"></asp:Label>
            </div>
        <div class="col-md-4">
            <p>Fra dato:</p>
            <asp:Calendar ID="Calendar1" runat="server"></asp:Calendar>
            </div>
            <div class="col-md-4">
                <p>Til dato:</p>
                <asp:Calendar ID="Calendar2" runat="server"></asp:Calendar>  
            </div>
        
        </div>
   

</asp:Content>

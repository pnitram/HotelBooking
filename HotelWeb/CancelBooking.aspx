<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CancelBooking.aspx.cs" Inherits="WebApplication1.CancelBooking" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    


    <div class="jumbotron">
        <h1>Kanseller booking!</h1>
            </div>

    <div class="row">
        <div class="col-md-5">
         <div>
                <br/>Tast inn din bookingreferanse:&nbsp;&nbsp;<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
             
            </div>
            <br/>
            <asp:Button ID="Button1" runat="server" CssClass="btn" OnClick="Button1_Click" Text="Kanseller" />
            </div>
        <div class="col-md-3">
        <asp:Label ID="Label3" runat="server" ></asp:Label>
        </div>
        </div>
    
   

</asp:Content>



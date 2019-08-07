<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OgrenciWebForm.aspx.cs" Inherits="ClassCreator.OgrenciWebForm" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta charset="utf - 8">
<meta http-equiv="Content - Type" content="text / html; charset = utf - 8"/>
<title></title>
<style type="text / css">
.auto-style1 { width:100%; } </style> </head> <body>
<link rel = "stylesheet" href = "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
<link rel = "stylesheet" href = "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css" />
<script src = "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" ></script>
<form id="form1" runat="server">
<asp:Repeater ID="rptOgrenci" runat="server">
<HeaderTemplate>
<table style="border:2px solid green" border="1">
<tr style="background-color:seagreen; color: white">
<td>ad</td>
<td>soyad</td>
<td>mail</td>
</tr>
</HeaderTemplate>
<ItemTemplate>
<tr>
<td><%#Eval("ad") %></td>
<td><%#Eval("soyad") %></td>
<td><%#Eval("mail") %></td>
</tr>
</ItemTemplate>
<FooterTemplate>
</table>
</FooterTemplate>
</asp:Repeater>
</form>
</body>
</html>

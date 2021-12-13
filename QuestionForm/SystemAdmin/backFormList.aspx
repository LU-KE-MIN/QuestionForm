<%@ Page Title="" Language="C#" MasterPageFile="~/SystemAdmin/back.Master" AutoEventWireup="true" CodeBehind="backFormList.aspx.cs" Inherits="QuestionForm.SystemAdmin.backFormList" %>

<%@ Register Src="~/UserControls/ucPagers.ascx" TagPrefix="uc1" TagName="ucPagers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div style="display: flex;justify-content: center;align-items: center; ">
    <table cellpadding="10" border="1" width="600">
        <tr>
            <td>
                <asp:Label ID="lblTitle" runat="server" Text="問卷標題"></asp:Label> &nbsp &nbsp
                <asp:TextBox ID="txtTitle" runat="server" style="width:350px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblDate" runat="server" Text="開始 / 結束"></asp:Label> &nbsp &nbsp
                <asp:TextBox ID="txtDateStart" runat="server" TextMode="Date"></asp:TextBox>
                <asp:TextBox ID="txtDateEnd" runat="server" TextMode="Date"></asp:TextBox> &nbsp &nbsp
                <asp:Button ID="btnSearch" runat="server" Text="搜尋" OnClick="btnSearch_Click" />
            </td>
        </tr>
    </table>
    </div>
    <br/>
    
     <div style="display: flex;justify-content: center;align-items: center; ">
    <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="../Images/trash.png" width="30" height="30" OnClick="btnDelete_Click" /> &nbsp
    <asp:ImageButton ID="btnNewForm" runat="server" ImageUrl="../Images/plus.png" width="30" height="30" OnClick="btnNewForm_Click" />
    </div>
    <asp:Literal ID="ltlMsg" runat="server"></asp:Literal>

     <div style="display: flex;justify-content: center;align-items: center; ">
   <asp:GridView class="table table-condensed" ID="gvAccountingList" runat="server" AlternatingRowStyle-Wrap="False" AutoGenerateColumns="False" OnRowDataBound="gvAccountingList_RowDataBound">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:CheckBox ID="ckbDelete" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="M_id" HeaderText="#" />
            <asp:HyperLinkField DataNavigateUrlFields="M_Guid" DataNavigateUrlFormatString="backDetail.aspx?ID={0}" DataTextField="M_title" HeaderText="問卷" />
            <asp:TemplateField HeaderText="狀態">
                <ItemTemplate>
                    <asp:Label ID="lblState" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="開始時間">
                <ItemTemplate>
                    <asp:Label ID="lblStartDate" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="結束時間">
                <ItemTemplate>
                    <asp:Label ID="lblEndDate" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="觀看統計">
                <ItemTemplate>
                    <a href="backDetail.aspx?ID=<%# Eval("M_Guid") %>#tabs4">前往</a>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    </div>

    <div style="display: flex;justify-content: center;align-items: center; ">
    <asp:Literal runat="server" ID="ltPager"></asp:Literal>
    </div>

    <div style="display: flex;justify-content: center;align-items: center; ">
    <uc1:ucPagers runat="server" id="ucPager" PageSize="10" CurrentPage="1" TotalSize="10" Url="backFormList.aspx" />
    </div>

    
</asp:Content>

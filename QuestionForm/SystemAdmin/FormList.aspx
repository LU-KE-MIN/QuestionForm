<%@ Page Title="" Language="C#" MasterPageFile="~/SystemAdmin/Site1.Master" AutoEventWireup="true" CodeBehind="FormList.aspx.cs" Inherits="QuestionForm.SystemAdmin.WebForm1" %>

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

    <asp:Literal ID="ltlMsg" runat="server"></asp:Literal>
</div>

    <div style="display: flex;justify-content: center;align-items: center;margin-top:1rem">
    <asp:GridView ID="gvAccountingList" runat="server" BackColor="White" AutoGenerateColumns="False" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" OnRowDataBound="gvAccountingList_RowDataBound" CellPadding="4" ForeColor="Black" GridLines="Vertical" Width="631px">
        <AlternatingRowStyle BackColor="#CCCCCC" />
        <Columns>
            <asp:BoundField HeaderText="#" DataField="M_id" />
            <asp:TemplateField HeaderText="問卷">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCaption"></asp:Label>
                                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="狀態">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblState"></asp:Label>
                                </ItemTemplate>
            </asp:TemplateField>
                        <asp:TemplateField HeaderText="開始時間">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStartDate"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                                    <asp:TemplateField HeaderText="結束時間">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEndDate"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
               <asp:TemplateField HeaderText="觀看統計">
                <ItemTemplate>
                    <a href="FormStatic.aspx?ID=<%# Eval("M_Guid") %>">前往</a>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <FooterStyle BackColor="#CCCCCC" />
        <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
        <SortedAscendingCellStyle BackColor="#F1F1F1" />
        <SortedAscendingHeaderStyle BackColor="#808080" />
        <SortedDescendingCellStyle BackColor="#CAC9C9" />
        <SortedDescendingHeaderStyle BackColor="#383838" />

    </asp:GridView>
</div>
    <div style="display: flex;justify-content: center;align-items: center;margin-top:0.5rem ">
    <asp:Literal runat="server" ID="ltPager"></asp:Literal>
    </div>

     <div style="display: flex;justify-content: center;align-items: center; ">
    <uc1:ucPagers runat="server" id="ucPager" PageSize="10" CurrentPage="1" TotalSize="10" Url="FormList.aspx" />
    </div>

    <div style="display: flex;justify-content: center;align-items: center; ">
     <asp:PlaceHolder ID="plcNoData" runat="server" Visible="false">
                        <p style="color: red; background-color: cornflowerblue">
                            No data in your Accounting Note.
                        </p>
                    </asp:PlaceHolder>
    </div>
</asp:Content>

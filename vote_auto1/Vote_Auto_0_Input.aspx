<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Vote_Auto_0_Input.aspx.cs" Inherits="vote_auto1.Vote_Auto_0_Input" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
       <p><b>自動化投票區</b></p><hr />
       <p>
           <span class="style3">開始出題</span>,請先輸入<b>投票題目</b>
           <asp:TextBox ID="TextBox_title" runat="server"></asp:TextBox>
           <asp:RequiredFieldValidator ID="RequiredFieldValidator_title" runat="server" ControlToValidate="TextBox_title" ErrorMessage="舉辦投票，怎能沒有題目?"></asp:RequiredFieldValidator>
           <br />
           <p>選項-1&nbsp; :<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
               <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox1" ErrorMessage="舉辦投票，至少要兩個選項"></asp:RequiredFieldValidator>
           </p>

            <p>選項-2&nbsp; :<asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
               <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextBox2" ErrorMessage="舉辦投票，至少要兩個選項"></asp:RequiredFieldValidator>
           </p>

           <p>選項-3&nbsp; :<asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
               <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TextBox3" ErrorMessage="舉辦投票，至少要兩個選項"></asp:RequiredFieldValidator>
           </p>

           <p>選項-4&nbsp; :<asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
               <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="TextBox4" ErrorMessage="舉辦投票，至少要兩個選項"></asp:RequiredFieldValidator>
           </p>
          
           &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
           <asp:Button ID="Button1" runat="server" Text="Button--完成投票的題目&選項" OnClick="Button1_Click" />
       </p><hr />

        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:ActiveQuestionConnectionString %>" 
            InsertCommand="INSERT INTO [Vote_Auto] ([Vote_time], [title], [question_1], [question_2], [question_3], [question_4],[question_all]) VALUES (@title, @question_1, @question_2, @question_3,, @question_4,)">

            <InsertParameters>
                <asp:ControlParameter ControlID="TextBox_title" Name="title" PropertyName="Text" Type="String" />
                <asp:ControlParameter ControlID="TextBox1" Name="question_1" PropertyName="Text" Type="String" />
                <asp:ControlParameter ControlID="TextBox2" Name="question_2" PropertyName="Text" Type="String" />
                <asp:ControlParameter ControlID="TextBox3" Name="question_3" PropertyName="Text" Type="String" />
                <asp:ControlParameter ControlID="TextBox4" Name="question_4" PropertyName="Text" Type="String" />
                
            </InsertParameters>
            
           
        </asp:SqlDataSource>
    </form>
</body>
</html>

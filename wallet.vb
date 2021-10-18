<%@ Import Namespace="System.Data" %>

<%@ Import Namespace="System.Data.OleDb" %>

<html>

<head>

    <title>Transfer Funds</title>

 

    <script language="C#" runat="server">

        protected void TransferFund(Object Sender, EventArgs e)

        {

            String strSQL = "Select dBalance FROM tblAccount where AccNumber='" + txtFrom.Text + "'";

            double dCurrBalance;

            OleDbConnection Conn = new OleDbConnection("PROVIDER=Microsoft.Jet.OLEDB.4.0;DATA

            SOURCE=c:\\inetpub\\wwwroot\\dotnet\\test.mdb;");

            Conn.Open();

            OleDbDataReader oReader;

            OleDbCommand cmd = new OleDbCommand(strSQL, Conn);

            OleDbTransaction Trans = Conn.BeginTransaction(IsolationLevel.ReadCommitted);

            cmd.Transaction = Trans;

            try

            {

                oReader = cmd.ExecuteReader();

                oReader.Read();

                dCurrBalance = oReader.GetDouble(0);

                oReader.Close();

                if (dCurrBalance < Convert.ToDouble(txtAmt.Text))

                {

                    throw (new Exception("Insufficient funds for transfer"));

                }

                strSQL = "Update tblAccount set dbalance =  dBalance - " + txtAmt.Text + " where AccNumber = '"

                + txtFrom.Text + "'";

                cmd.CommandText = strSQL;

                cmd.ExecuteNonQuery();

                strSQL = "Update tblAccount set dbalance =  dBalance + " + txtAmt.Text + " where AccNumber = '"

                + txtTo.Text + "'";

                cmd.CommandText = strSQL;

                cmd.ExecuteNonQuery();

                Trans.Commit();

                lbl.Text = "true";

            }

            catch (Exception ex)

            {

                Trans.Rollback();

                lbl.Text = "Error: " + ex.Message;

            }

            finally

            {

                Conn.Close();

            } 

        } 

 

    </script>

 

</head>

<body>

    <form id="frmTransfer" runat="server">

        <asp:Label ID="lblFrom" runat="server">Enter the account number from which to transfer

          funds</asp:Label>

        <asp:TextBox ID="txtFrom" runat="server"></asp:TextBox><br />

        <asp:Label ID="lblTo" runat="server">Enter the account number to which to transfer funds</asp:Label>

        <asp:TextBox ID="txtTo" runat="server"></asp:TextBox><br />

        <asp:Label ID="lblAmount" runat="server">Enter the amount to transfer</asp:Label>

        <asp:TextBox ID="txtAmt" runat="server"></asp:TextBox><br />

        <asp:Button ID="Button1" OnClick="TransferFund" runat="server" Text="Start Transfer">

        </asp:Button><br />

        <asp:Label ID="lbl" runat="server"></asp:Label>

    </form>

</body>

</html>
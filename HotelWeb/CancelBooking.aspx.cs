using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HashidsNet;
using MySql.Data.MySqlClient;

namespace WebApplication1
{
    public partial class CancelBooking : System.Web.UI.Page
    {
        private string _myconnectionstring;
        private string _bookingReferanse;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            if (TextBox1.Text == string.Empty)
            {
                Label3.Text = @"</br>Du må taste inn en booking referanse!";
                return;
            }
            
            try
            {
                _bookingReferanse = TextBox1.Text;
                _myconnectionstring = "Database=Test; Data Source = localhost; User = test; Password = test";
                var hashids = new Hashids("Tralalalalala, dette er min SALT");
                var bid = hashids.Decode(_bookingReferanse);
               using (MySqlConnection cn = new MySqlConnection(_myconnectionstring))
                {
                    var sql = "DELETE FROM booking WHERE bookingId=" +bid[0];
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandText = sql;
                    cn.Open();
                    int numRowsUpdated = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    TextBox1.Text = string.Empty;
                    if (numRowsUpdated == 1)
                    {
                        Label3.Text = "</br>Takk! Bookingen med booking referanse:  \"" + _bookingReferanse + "\". ble kansellert.</br> " + numRowsUpdated + " rad i tabellen ble berørt";
                    }
                    else
                    {
                        Label3.Text = "</br>Bookingen med booking referanse:  \"" + _bookingReferanse + "\" finnes ikke.</br>";
                    }
                    
                }
            }

            catch
            {
                Label3.Text = "</br>Bookingen med booking referanse:  \"" + _bookingReferanse + "\" finnes ikke.</br>";
                Console.WriteLine(@"Error: DB not updated!");
            }
        }
    }
}
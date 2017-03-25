﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotellBookWF;
using MySql.Data.MySqlClient;

namespace WebApplication1
{
    public partial class _Default : Page
    {
        private string _myconnectionstring;


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            _myconnectionstring = "Database=Test; Data Source = localhost; User = test; Password = test";

            if (TextBox1.Text == string.Empty || TextBox2.Text == string.Empty)
            {
                Label3.Text = @"Du må taste inn både fornavn og etternavn!";
                return;
            }

            else if (Calendar1.SelectedDate < DateTime.Now.Date)
            {
                Label3.Text = @"Fra dato kan ikke være før dagens dato!";
                return;
            }

            else if (Calendar2.SelectedDate < Calendar1.SelectedDate)
            {
                Label3.Text = @"Du kan ikke velge en til dato som er tidligere enn fra dato!";
                return;
            }

            try
            {
                using (MySqlConnection cn = new MySqlConnection(_myconnectionstring))
                {
                    var sql = "INSERT INTO booking (fornavn, etternavn, datoFra, datoTil) VALUES(" + '"' + TextBox1.Text + '"' + "," + '"' + TextBox2.Text +'"' + "," + '"' + Calendar1.SelectedDate.ToString("yyyy-MM-dd") + '"' + "," + '"' + Calendar2.SelectedDate.ToString("yyyy-MM-dd") + '"' + ")";
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandText = sql;
                    cn.Open();
                    int numRowsUpdated = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    TextBox1.Text = string.Empty;
                    TextBox2.Text = String.Empty;
                    Calendar1.SelectedDates.Clear();
                    Calendar2.SelectedDates.Clear();
                    Label3.Text = "Takk! Bookingen ble registrert. " + numRowsUpdated + " rad lagt til.";
                }
            }

            catch
            {
                Console.WriteLine(@"Error: DB not updated!");
            }
            //TextBox1.Text = Calendar1.SelectedDate.ToString("yyyy-M-dd");
        }
        
    }
}
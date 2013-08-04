﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;

using System.Web.Services;
using CodeCarvings;

namespace POS.views
{
    public partial class section : System.Web.UI.Page
    {
        public static char IU = 'I';

        public static System.Web.UI.Page myPageInstance = null;
        public static SimpleImageUpload imgUpload = null;
        public static ListView myList = null;
        public static UpdatePanel myUpdatePanel = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            myPageInstance = this;
            imgUpload = ImageUpload1;
            myList = lstvSection;
            myUpdatePanel = UpdatePanel1;

            ListLoad();
            DrpListLoad();


            
            if (!this.IsPostBack)
            {
                string fileName = "dummy";
                string sourceImageFilePath = "~/uploadedImg/" + fileName + ".jpg";
                imgUpload.LoadImageFromFileSystem(sourceImageFilePath);

                //imgUpload.UnloadImage();

            }
        }

        private void DrpListLoad()
        {
            myList.Items.Clear();

            string DBConnectionString = ConfigurationManager.ConnectionStrings["POSDB"].ConnectionString;
            SqlConnection con = new SqlConnection(DBConnectionString);

            string sqlCmd = "testSelectInsertUpdateDelete";
            using (SqlCommand cmd = new SqlCommand(sqlCmd, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StatementType", "Select");
                cmd.Parameters.Add("@NewId", SqlDbType.Int).Direction = ParameterDirection.Output;

                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                con.Open();
                adpt.Fill(ds);
                con.Close();

                deptDrplst.DataSource = ds.Tables[0];
                deptDrplst.DataValueField = "deptID";
                deptDrplst.DataTextField = "deptName";
                deptDrplst.DataBind();
                deptDrplst.Items.Insert(0, new ListItem("Select One", "-1"));
            }
        }

        private static void ListLoad()
        {
            myList.Items.Clear();

            string DBConnectionString = ConfigurationManager.ConnectionStrings["POSDB"].ConnectionString;
            SqlConnection con = new SqlConnection(DBConnectionString);

            string sqlCmd = "sectionSelectInsertUpdateDelete";
            using (SqlCommand cmd = new SqlCommand(sqlCmd, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StatementType", "Select");
                cmd.Parameters.Add("@NewId", SqlDbType.Int).Direction = ParameterDirection.Output;

                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                con.Open();
                adpt.Fill(ds);
                con.Close();

                myList.DataSource = ds;
                myList.DataBind();
            }
        }

        [WebMethod]
        public static void saveImage(int newID)
        {
            string fileName = newID.ToString();

            if (imgUpload.HasNewImage)
            {
                imgUpload.SaveProcessedImageToFileSystem("~/uploadedImg/section/" + fileName + ".jpg");
            }

            ListLoad();
        }

        [WebMethod]
        public static void updateRow(int id)
        {
            imgUpload.UnloadImage();


            imgUpload.LoadControl("~/piczardUserControls/simpleImageUploadUserControl/SimpleImageUpload.ascx");

            string fileName = id.ToString();
            string sourceImageFilePath = "~/uploadedImg/section/" + fileName + ".jpg";
            imgUpload.LoadImageFromFileSystem(sourceImageFilePath);


            imgUpload.LoadControl("~/piczardUserControls/simpleImageUploadUserControl/SimpleImageUpload.ascx");

            ListLoad();
        }

    }
}
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
    public partial class department : System.Web.UI.Page
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
            myList = lstvDept;
            myUpdatePanel = UpdatePanel1;

            ListLoad();

            

            

            if (!this.IsPostBack)
            {
                string fileName = "dummy";
                string sourceImageFilePath = "~/uploadedImg/" + fileName + ".jpg";
                imgUpload.LoadImageFromFileSystem(sourceImageFilePath);
                
                //imgUpload.UnloadImage();
                
            }
            

            
        }

        private static void ListLoad()
        {
            myList.Items.Clear();

            string DBConnectionString = ConfigurationManager.ConnectionStrings["POSDB"].ConnectionString;
            SqlConnection con = new SqlConnection(DBConnectionString);

            string sqlCmd = "TestSelectInsertUpdateDelete";
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
                imgUpload.SaveProcessedImageToFileSystem("~/uploadedImg/department/" + fileName + ".jpg");
            }

            ListLoad();
        }

        [WebMethod]
        public static void updateRow(int id)
        {
            imgUpload.UnloadImage();

            
            imgUpload.LoadControl("~/piczardUserControls/simpleImageUploadUserControl/SimpleImageUpload.ascx");

            string fileName = id.ToString();
            string sourceImageFilePath = "~/uploadedImg/department/" + fileName + ".jpg";
            imgUpload.LoadImageFromFileSystem(sourceImageFilePath);
            
            
            imgUpload.LoadControl("~/piczardUserControls/simpleImageUploadUserControl/SimpleImageUpload.ascx");
            
            ListLoad();
        }

        [WebMethod]
        public static void SendForm(int newID)
        {
            string fileName = newID.ToString();

            //saveImage(newID);
            if (imgUpload.HasNewImage)
            {
                imgUpload.SaveProcessedImageToFileSystem("~/uploadedImg/department/" + fileName + ".jpg");
            }
        }

        

        protected void Button1_Click(object sender, EventArgs e)
        {
            string fileName = sender.ToString();
            fileName = "dept1";

            if (ImageUpload1.HasNewImage)
            {
                ImageUpload1.SaveProcessedImageToFileSystem("~/uploadedImg/department/" + fileName + ".jpg");
            }

            ListLoad();
        }

        protected void onImageUpload(object sender, EventArgs e)
        {
            if (ImageUpload1.HasNewImage)
            {
                //ImageUpload1.SaveProcessedImageToFileSystem("~/uploadedImg/MyImage.jpg");
            }
        }

        protected void onDataBinding(object sender, EventArgs e)
        {
            if (ImageUpload1.HasNewImage)
            {
                //ImageUpload1.SaveProcessedImageToFileSystem("~/uploadedImg/MyImage.jpg");
            }
        }

        protected void lstvDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ListView.SelectedListViewItemCollection breakfast = lstvDept.selectedI.SelectedItems;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;

//using Geo.Utils;

namespace Geo.Winform
{
    /// <summary>
    /// 文件类型
    /// </summary>
    public enum ImageSizeType { Size16x16, Size48x48 }
    public enum ImageType { Menu, FileType }


    /// <summary>
    /// 图标管理器。
    /// </summary>
    public class ImageManager
    {
        public const string DEFAULT = "default";
        public const string SELECTED = "selceted";
        public const string TOBE_MOVE = "tobe_move";
        public const string PROJECT = "project";
        public const string PROJECT_SELECTED = "project_selected";
        public const string SITE_POINT = "site_point";
        public const string SITE_POINT_SELECTED = "site_point_selected";
        public const string PROJECT_FILE = "project_file";
        public const string PROJECT_FILE_SELECTED = "project_file_selected";
        public const string PROJECT_POSITION = "project_position";
        public const string PROJECT_POSITION_SELECTED = "project_position_selected";

        public const string CONTROL_POINT = "control_point";
        public const string CONTROL_POINT_SELECTED = "control_point_selected";

        public const string NET_POINT = "net_point";
        public const string NET_POINT_SELECTED = "net_point_selected";

        public const string USER_DEFAULT = "user_default";
        public const string USER_SELECTED = "user_selected";


        private static ImageManager instance = new ImageManager();
        /// <summary>
        /// 图标管理器。
        /// </summary>
        public static ImageManager Instance
        {
            get { return instance; }
        }

        private ImageManager()
        {
            ImageDirPath = Geo.Utils.FileUtil.GetAssemblyFolderPath() + "\\Data\\Images";//".\\Images";;//
             IconPath = ImageDirPath + "\\Icons";

            DefaultPhotoPath = ImageDirPath + "\\Photos\\defaultPhoto.jpg";

            IconFileTypePath = IconPath + "\\FileType";
            IconFileTypePath48x48 = IconFileTypePath + "\\48x48";
            IconFileTypePath16x16 = IconFileTypePath + "\\16x16";

            IconMenuPath = IconPath + "\\Menu";
            IconMenuPath48x48 = IconMenuPath + "\\48x48";
            IconMenuPath16x16 = IconMenuPath + "\\16x16";
            //载入图片
            try
            {
                DefaultPhoto = new Bitmap(DefaultPhotoPath);

                //用户单位图标
                UnitImages16x16 = new ImageList();
                UnitImages16x16.Images.Add(DEFAULT, GetMenuIcon("group_16x16", ImageSizeType.Size16x16));
                UnitImages16x16.Images.Add(SELECTED, GetMenuIcon("group2_16x16", ImageSizeType.Size16x16));
                UnitImages16x16.Images.Add(TOBE_MOVE, GetMenuIcon("group_16x16", ImageSizeType.Size16x16));
                UnitImages16x16.Images.Add(USER_DEFAULT, GetMenuIcon("user_16x16", ImageSizeType.Size16x16));
                UnitImages16x16.Images.Add(USER_SELECTED, GetMenuIcon("user_selected_16x16", ImageSizeType.Size16x16));



                //文件夹图标
                DirectoryImageListSmall = new ImageList();
                DirectoryImageListSmall.Images.Add(DEFAULT, GetMenuIcon("folder_closed_16x16", ImageSizeType.Size16x16));
                DirectoryImageListSmall.Images.Add(SELECTED, GetMenuIcon("folder_open_16x16", ImageSizeType.Size16x16));
                DirectoryImageListSmall.Images.Add(TOBE_MOVE, GetMenuIcon("folder_cutting_16x16", ImageSizeType.Size16x16));
                DirectoryImageListSmall.Images.Add(PROJECT, GetMenuIcon("project_16x16", ImageSizeType.Size16x16));
                DirectoryImageListSmall.Images.Add(PROJECT_SELECTED, GetMenuIcon("project_selected_16x16", ImageSizeType.Size16x16));
                DirectoryImageListSmall.Images.Add(SITE_POINT, GetMenuIcon("site_point_16x16", ImageSizeType.Size16x16));
                DirectoryImageListSmall.Images.Add(SITE_POINT_SELECTED, GetMenuIcon("site_point_selected_16x16", ImageSizeType.Size16x16));
                DirectoryImageListSmall.Images.Add(PROJECT_FILE, GetMenuIcon("project_file_16x16", ImageSizeType.Size16x16));
                DirectoryImageListSmall.Images.Add(PROJECT_FILE_SELECTED, GetMenuIcon("project_file_selected_16x16", ImageSizeType.Size16x16));
                DirectoryImageListSmall.Images.Add(PROJECT_POSITION, GetMenuIcon("project_position_16x16", ImageSizeType.Size16x16));
                DirectoryImageListSmall.Images.Add(PROJECT_POSITION_SELECTED, GetMenuIcon("project_position_selected_16x16", ImageSizeType.Size16x16));
                DirectoryImageListSmall.Images.Add(CONTROL_POINT, GetMenuIcon(CONTROL_POINT + "_16x16", ImageSizeType.Size16x16));
                DirectoryImageListSmall.Images.Add(CONTROL_POINT_SELECTED, GetMenuIcon(CONTROL_POINT_SELECTED + "_16x16", ImageSizeType.Size16x16));
                DirectoryImageListSmall.Images.Add(NET_POINT, GetMenuIcon(NET_POINT + "_16x16", ImageSizeType.Size16x16));
                DirectoryImageListSmall.Images.Add(NET_POINT_SELECTED, GetMenuIcon(NET_POINT_SELECTED+"_16x16", ImageSizeType.Size16x16));

                string[] imageNames = {
                                          "chm", "jpg", "jpeg", "dir", "dir_open", "doc",
                                          "dir", "docx", "exe", "exe_install", "gif",
                                          "png", "ppt", "pptx", "rar", "unknown", 
                                          "xls", "xlsx", "zip", "pdf", "txt", "iso" ,"caj", "kdh"
                                      };

                //文件图标 16 *16
                FileImageListSmall = new ImageList();
                FileImageListSmall.ImageSize = new Size(16, 16);
                foreach (string typeName in imageNames)
                {
                    AddImage(FileImageListSmall, typeName, ImageSizeType.Size16x16);
                }

                //文件图标 48 x 48
                FileImageListLarge = new ImageList();
                FileImageListLarge.ImageSize = new Size(48, 48);
                foreach (string typeName in imageNames)
                {
                    AddImage(FileImageListLarge, typeName, ImageSizeType.Size48x48);
                }
            }
            catch (Exception ex)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("载入图片出错：" + this.GetType() + ex.Message);
            }
        }

        private void AddImage(ImageList list, string imageNameWithoutExtension, ImageSizeType imageSizeType)
        {
            string key = imageNameWithoutExtension;
            Image image = GetFileTypeIcon(imageNameWithoutExtension, imageSizeType);
            list.Images.Add(key, image);
        }
        #region 内部变量
        private string ImageDirPath;
        private string IconPath;

        private string IconFileTypePath;
        private string IconFileTypePath48x48;
        private string IconFileTypePath16x16;

        private string IconMenuPath;
        private string IconMenuPath48x48;
        private string IconMenuPath16x16;
        #endregion
        
        #region 公开属性

        public ImageList UnitImages16x16{ get; set; }

        ///<summary>
        /// Create two ImageList objects. 
        /// 文件目录小图标
        /// </summary>
        public ImageList DirectoryImageListSmall { get; set; }
        /// <summary>
        /// 文件目录大图标
        /// </summary>
        public ImageList DirectoryImageListLarge { get; set; }

        /// <summary>
        /// 文件大图标
        /// </summary>
        public ImageList FileImageListLarge { get; set; }
        /// <summary>
        /// 文件小图标
        /// </summary>
        public ImageList FileImageListSmall{ get; set; }
        #endregion

        /// <summary>
        /// 文件类型图标
        /// </summary>
        /// <param name="fileExtension"></param>
        /// <param name="sizeType"></param>
        /// <returns></returns>
        public Image GetFileTypeIcon(string fileExtension, ImageSizeType sizeType)
        {
            string extName = fileExtension.Replace(".","").Trim();
            string path = "";
            switch (sizeType)
            {
                case ImageSizeType.Size16x16:
                    path = IconFileTypePath16x16;  break;
                case ImageSizeType.Size48x48:
                   path = IconFileTypePath48x48; break;
                default:
                     path = IconFileTypePath16x16; break;
            }
            path += "\\" + extName + ".png";
            return Bitmap.FromFile(path);
        }
        /// <summary>
        /// 按钮图标类型
        /// </summary>
        /// <param name="menuName"></param>
        /// <param name="sizeType"></param>
        /// <returns></returns>
        public Image GetMenuIcon(string menuName, ImageSizeType sizeType)
        {
            string name = menuName.Trim();
            switch (sizeType)
            {
                case ImageSizeType.Size16x16:
                    return Bitmap.FromFile(IconMenuPath16x16 + "\\" + name + ".png");
                case ImageSizeType.Size48x48:
                    return Bitmap.FromFile(IconMenuPath48x48 + "\\" + name + ".png");
                default:
                    return Bitmap.FromFile(IconMenuPath16x16 + "\\" + name + ".png");
            }
        }


        public static string DefaultPhotoPath { get; set; }

        public static Image DefaultPhoto { get; set; }

        public ImageList GetImageListSmall(TreeType topTreeType)
        {
            ImageList imageListSmall = this.DirectoryImageListSmall;
            switch (topTreeType)
            {
                case Geo.TreeType.文档节点:
                    break;
                case Geo.TreeType.材料节点:
                    break;
                case Geo.TreeType.工程节点:
                    break;
                case Geo.TreeType.供应商节点:
                    break;
                case Geo.TreeType.门店节点:
                    break;
                case Geo.TreeType.用户单位:
                    imageListSmall = UnitImages16x16;
                    break;
                default:
                    imageListSmall = this.DirectoryImageListSmall;
                    break;
            }
            return imageListSmall;
        }

    }
}

﻿using System;
using System.Threading.Tasks;
using Foundation;
using MJRefresh;
using UIKit;

namespace Test_MJRefresh
{
    public partial class ViewController : UIViewController
    {
        public UITableView table;
        public int Count { get; set; }

        public ViewController()
        {
            
        }

        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.View.BackgroundColor = UIColor.White;

            table = new UITableView();
            table.Frame = new CoreGraphics.CGRect(0, 40, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height - 64 - 40);
            table.DataSource = new TableDataSource(this);
            table.Delegate = new TableDelegate(this);
            this.View.AddSubview(table);

            MJRefreshNormalHeader header = new MJRefreshNormalHeader();
            table.SetHeader(header);

            MJRefreshAutoNormalFooter footer = new MJRefreshAutoNormalFooter();
            table.SetFooter(footer);

            header.RefreshingBlock = async () => {
                await Task.Delay(2000);
                InvokeOnMainThread(() => {
                    footer.Hidden = true;
                    this.Count += 12;
                    table.ReloadData();
                    table.Header().EndRefreshing();
                    footer.Hidden = false;

                });
            };

            header.SetTitle(NSBundle_MJRefresh.Mj_localizedStringForKey(NSBundle_MJRefresh.Mj_refreshBundle(NSBundle.MainBundle), "MJRefreshHeaderIdleText"), MJRefreshState.Idle);
            header.SetTitle(NSBundle_MJRefresh.Mj_localizedStringForKey(NSBundle_MJRefresh.Mj_refreshBundle(NSBundle.MainBundle), "MJRefreshHeaderPullingText"), MJRefreshState.Pulling);
            header.SetTitle(NSBundle_MJRefresh.Mj_localizedStringForKey(NSBundle_MJRefresh.Mj_refreshBundle(NSBundle.MainBundle), "MJRefreshHeaderRefreshingText"), MJRefreshState.Refreshing);

            header.AutomaticallyChangeAlpha = true;


            footer.RefreshingBlock = async () => {
                await Task.Delay(2000);
                InvokeOnMainThread(() => {
                    footer.Hidden = true;
                    this.Count += 5;
                    table.ReloadData();
                    table.Footer().EndRefreshing();
                    footer.Hidden = false;
                });
            };

            footer.SetTitle(NSBundle_MJRefresh.Mj_localizedStringForKey(NSBundle_MJRefresh.Mj_refreshBundle(NSBundle.MainBundle), "MJRefreshAutoFooterIdleText"), MJRefreshState.Idle);
            footer.SetTitle(NSBundle_MJRefresh.Mj_localizedStringForKey(NSBundle_MJRefresh.Mj_refreshBundle(NSBundle.MainBundle), "MJRefreshAutoFooterRefreshingText"), MJRefreshState.Refreshing);
            footer.SetTitle(NSBundle_MJRefresh.Mj_localizedStringForKey(NSBundle_MJRefresh.Mj_refreshBundle(NSBundle.MainBundle), "MJRefreshAutoFooterNoMoreDataText"), MJRefreshState.NoMoreData);



            table.Header().BeginRefreshing();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }

    public class TableDelegate : UITableViewDelegate
    {
        private ViewController _vc;
        public TableDelegate(ViewController vc)
        {
            _vc = vc;
        }
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            _vc.NavigationController.PushViewController(new MJTableViewController(), true);
        }
    }

    public class TableDataSource : UITableViewDataSource
    {
        private ViewController _vc;
        public TableDataSource(ViewController vc)
        {
            _vc = vc;
        }
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell("testcell");
            if (cell == null)
                cell = new UITableViewCell(UITableViewCellStyle.Default, "testcell");
            if(indexPath.Row % 2 != 0)
                cell.TextLabel.Text = "push";
            else
                cell.TextLabel.Text = "modal";
            return cell;
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return _vc.Count;
        }


    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using LottieUWP;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace LottieUwpTest
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            var localDir = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
            var file = await localDir.GetFileAsync("test.json");
            var json = await FileIO.ReadTextAsync(file, Windows.Storage.Streams.UnicodeEncoding.Utf8);

            _drawForCanvas = new LottieDrawable();

            await LottieView.SetAnimationFromJsonAsync(json);
            var composition =await LottieComposition.Factory.FromJsonStringAsync(json);
            LottieDrawable.SetComposition(composition);
            LottieDrawable.RepeatCount = -1;
            _drawForCanvas.SetComposition(composition);
            _drawForCanvas.RepeatCount = -1;
            _inited = true;

            LottieView.PlayAnimation();
            LottieDrawable.PlayAnimation();
            _drawForCanvas.PlayAnimation();
        }

        private LottieDrawable _drawForCanvas;
        private bool _inited=false;
        private void CanvasAnimatedControl_Draw(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedDrawEventArgs args)
        {
            if (_inited)
            {
                var commandList = _drawForCanvas.GetCanvasImage(sender,1,1);
                args.DrawingSession.DrawImage(commandList);
            }
        }
    }
}

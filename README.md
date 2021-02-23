# Xamarin Forms UI Plugin for Android, iOS
It's compatible with portable projects as well as .Net standard projects.

Getting Started
===============

1. Install Symtech.Xamarin.UI to PCL project as well as client projects
2. Initialise Symtech.Xamarin.UI in MainAcitvity.cs and AppDelegate.cs:

MainActivity.cs
---------------
```c#
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Symtech.Xamarin.UI.Plugin.Init(this);
            LoadApplication(new App());
        }
```

AppDelegate.cs
---------------
```c#
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            Symtech.Xamarin.UI.iOS.Plugin.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
```

3. Add namespace to XAML:  `xmlns:controls="clr-namespace:Symtech.Xamarin.UI.Controls;assembly=Symtech.Xamarin.UI"`
4. Use the UI component from the plugin:
```xaml
<controls:FancyEntry x:Name="EmailEntry" Title="Email" Style="{StaticResource FancyEntry}" Keyboard="Email" ReturnType="Next"/>
```

Usage
=====
Refer to sample project
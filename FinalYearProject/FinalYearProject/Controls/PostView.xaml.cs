using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FinalYearProject.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostView : ContentView
    {
        public PostView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty AvatarUsernameProperty = BindableProperty.Create(
            propertyName: nameof(AvatarUsername),
            returnType: typeof(string),
            declaringType: typeof(PostView),
            defaultValue: default(string));

        public string AvatarUsername
        {
            get => (string)GetValue(AvatarUsernameProperty);
            set => SetValue(AvatarUsernameProperty, value);
        }

        public static readonly BindableProperty AvatarColourProperty = BindableProperty.Create(
            propertyName: nameof(AvatarColour),
            returnType: typeof(Color),
            declaringType: typeof(PostView),
            defaultValue: default(Color));

        public Color AvatarColour
        {
            get => (Color)GetValue(AvatarColourProperty);
            set => SetValue(AvatarColourProperty, value);
        }

        public static readonly BindableProperty TimestampProperty = BindableProperty.Create(
            propertyName: nameof(Timestamp),
            returnType: typeof(string),
            declaringType: typeof(PostView),
            defaultValue: default(string));

        public string Timestamp
        {
            get => (string)GetValue(TimestampProperty);
            set => SetValue(AvatarUsernameProperty, value);
        }

        public static readonly BindableProperty PostTextProperty = BindableProperty.Create(
            propertyName: nameof(PostText),
            returnType: typeof(string),
            declaringType: typeof(PostView),
            defaultValue: default(string));

        public string PostText
        {
            get => (string)GetValue(PostTextProperty);
            set => SetValue(AvatarUsernameProperty, value);
        }

        public static readonly BindableProperty UpdateLikeCommandProperty = BindableProperty.Create(
            propertyName: nameof(UpdateLikeCommand),
            returnType: typeof(ICommand),
            declaringType: typeof(PostView));

        public ICommand UpdateLikeCommand
        {
            get => (ICommand)GetValue(UpdateLikeCommandProperty);
            set => SetValue(UpdateLikeCommandProperty, value);
        }

        public static readonly BindableProperty UpdateLikeCommandParameterProperty = BindableProperty.Create(
            propertyName: nameof(UpdateLikeCommandParameter),
            returnType: typeof(object),
            declaringType: typeof(PostView),
            defaultValue: default);

        public object UpdateLikeCommandParameter
        {
            get => GetValue(UpdateLikeCommandParameterProperty);
            set => SetValue(UpdateLikeCommandParameterProperty, value);
        }

        public static BindableProperty IsLikedByUserProperty = BindableProperty.Create(
            propertyName: nameof(IsLikedByUser),
            returnType: typeof(bool),
            declaringType: typeof(PostView));

        public bool IsLikedByUser
        {
            get { return (bool)GetValue(IsLikedByUserProperty); }
            set { SetValue(IsLikedByUserProperty, value); }
        }

        public static readonly BindableProperty LikeCountProperty = BindableProperty.Create(
            propertyName: nameof(LikeCount),
            returnType: typeof(int),
            declaringType: typeof(PostView));

        public int LikeCount
        {
            get => (int)GetValue(LikeCountProperty);
            set => SetValue(LikeCountProperty, value);
        }

        public static readonly BindableProperty ReplyCommandProperty = BindableProperty.Create(
            propertyName: nameof(ReplyCommand),
            returnType: typeof(ICommand),
            declaringType: typeof(PostView));

        public ICommand ReplyCommand
        {
            get => (ICommand)GetValue(ReplyCommandProperty);
            set => SetValue(ReplyCommandProperty, value);
        }

        public static readonly BindableProperty ReplyCommandParameterProperty = BindableProperty.Create(
            propertyName: nameof(ReplyCommandParameter),
            returnType: typeof(object),
            declaringType: typeof(PostView),
            defaultValue: default);

        public object ReplyCommandParameter
        {
            get => GetValue(ReplyCommandParameterProperty);
            set => SetValue(ReplyCommandParameterProperty, value);
        }

        public static readonly BindableProperty ReplyCountProperty = BindableProperty.Create(
            propertyName: nameof(ReplyCount),
            returnType: typeof(int),
            declaringType: typeof(PostView));

        public int ReplyCount
        {
            get => (int)GetValue(ReplyCountProperty);
            set => SetValue(ReplyCountProperty, value);
        }

        public static readonly BindableProperty ReportCommandProperty = BindableProperty.Create(
            propertyName: nameof(ReportCommand),
            returnType: typeof(ICommand),
            declaringType: typeof(PostView));

        public ICommand ReportCommand
        {
            get => (ICommand)GetValue(ReportCommandProperty);
            set => SetValue(ReportCommandProperty, value);
        }

        public static readonly BindableProperty ReportCommandParameterProperty = BindableProperty.Create(
            propertyName: nameof(ReportCommandParameter),
            returnType: typeof(object),
            declaringType: typeof(PostView),
            defaultValue: default);

        public object ReportCommandParameter
        {
            get => GetValue(ReportCommandParameterProperty);
            set => SetValue(ReportCommandParameterProperty, value);
        }

        public static BindableProperty HasReplyIconProperty = BindableProperty.Create(
            propertyName: nameof(HasReplyIcon),
            returnType: typeof(bool),
            declaringType: typeof(PostView),
            defaultValue: true);

        public bool HasReplyIcon
        {
            get { return (bool)GetValue(HasReplyIconProperty); }
            set { SetValue(HasReplyIconProperty, value); }
        }
    }
}
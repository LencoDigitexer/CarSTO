using CarSTO.Commands;
using CarSTO.Models;
using CarSTO.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Drawing.Printing;
using System.Diagnostics;
using System.Windows.Documents;

namespace CarSTO.ViewModels
{
    internal class CarViewModel : ViewModel
    {

        string? oldImage;
        string? newImage;
        string? newImagePath;

        #region Команда ExitCommand
        public ICommand ExitCommand { get; set; }

        private bool CanExitCommandExecute(object p)
        {
            return true;
        }

        private void OnExitCommandExecuted(object p)
        {
            //MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            Window window = Application.Current.Windows[0];

           
            //MessageBox.Show(window.Name.ToString());

            LoginWindow loginWindow = new LoginWindow();
            LoginViewModel loginViewModel = new LoginViewModel();
            loginWindow.DataContext = loginViewModel;
            
            
            window.Close();
            loginWindow.Show();
            

        }
        #endregion

        #region Команда DeleteCommand
        public ICommand DeleteCommand { get; set; }

        private bool CanDeleteCommandExecute(object p)
        {
            if ((Cars)p != null)
            {
                return true;
            }
            else {
                return false;
            }
            //return true;
        }

        private void OnDeleteCommandExecuted(object p)
        {
            using (CarSTOContext db = new CarSTOContext())
            {
                

                if (CanDeleteProduct( (Cars)p  ) )
                {

                    if ((Cars)p != null)
                    {

                        if (MessageBox.Show($"Вы точно хотите удалить {((Cars)p).Gosnomer}", "Внимание!",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            db.Car.Remove((Cars)p);
                            db.SaveChanges();
                            MessageBox.Show($"Автомобиль { ((Cars)p).Gosnomer} удален!");
                            
                            CurrentProducts = db.Car.ToList();
                            CountProducts = $"Количество: {CurrentProducts.Count()} из {db.Car.ToList().Count}";
                        }

                    }

                }

            }
        }
        #endregion

        #region Команда EditProductCommand
        public ICommand EditProductCommand { get; set; }

        private bool CanEditProductCommandExecute(object p)
        {

            if (CurrentRole.Contains("Администратор"))
            {
                return true;
            }
            else
            {
                return false;
            }

            //return true;
            
        }

        private void OnEditProductCommandExecuted(object p)
        {
            //MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            AddProductWindow addProductWindow = new AddProductWindow();
            addProductWindow.DataContext = this;
            CurrentProduct = (Cars)p;
            Title = $"Редактирование {CurrentProduct.Vladelec}";

            addProductWindow.ShowDialog();
        }
        #endregion

        #region Команда SaveProductCommand
        public ICommand SaveProductCommand { get; set; }

        private bool CanSaveProductCommandExecute(object p)
        {
            return true;
        }

        private void OnSaveProductCommandExecuted(object p)
        {
            // валидация 

            #region Валидация
            //StringBuilder errors = new StringBuilder();
            //if (string.IsNullOrWhiteSpace(CurrentProduct.ArticleNumber))
            //    errors.AppendLine("Укажите артикль");
            //if (string.IsNullOrWhiteSpace(nameBox.Text))
            //    errors.AppendLine("Укажите название");
            //if (string.IsNullOrWhiteSpace(descriptionBox.Text))
            //    errors.AppendLine("Укажите описание");
            //if (string.IsNullOrWhiteSpace(categoryBox.Text))
            //    errors.AppendLine("Укажите категорию");
            //if (string.IsNullOrWhiteSpace(manufacturerBox.Text))
            //    errors.AppendLine("Укажите производителя");
            //if (string.IsNullOrWhiteSpace(costBox.Text))
            //    errors.AppendLine("Укажите цену");

            //if (string.IsNullOrWhiteSpace(discountAmountBox.Text))
            //    errors.AppendLine("Укажите размер скидки");
            //if (string.IsNullOrWhiteSpace(quantityInStockBox.Text))
            //    errors.AppendLine("Укажите количество на складе");
            //if (string.IsNullOrWhiteSpace(statusBox.Text))
            //    errors.AppendLine("Укажите статус");
            //if (string.IsNullOrWhiteSpace(maxDiscountBox.Text))
            //    errors.AppendLine("Укажите максимальную скидку");
            //if (string.IsNullOrWhiteSpace(supplierBox.Text))
            //    errors.AppendLine("Укажите поставщика");
            //if (string.IsNullOrWhiteSpace(unitBox.Text))
            //    errors.AppendLine("Укажите единицы измерения");

            //
            //if (errors.Length > 0)
            //{
            //    MessageBox.Show(errors.ToString());
            //    return;
            //}
            #endregion

            using (CarSTOContext db = new CarSTOContext())
            {

                if (CurrentProduct.Id == null)
                {
                    //MessageBox.Show("Добавляем машину?");
                    try
                    {
                        Cars product = new Cars()
                        {
                            //ArticleNumber = articleBox.Text,
                            //Name = nameBox.Text,
                            //Description = descriptionBox.Text,
                            //Category = categoryBox.Text,
                            //Photo = imageBox.Text, // "picture.png",
                            //Manufacturer = manufacturerBox.Text,
                            //Cost = Convert.ToDecimal(costBox.Text),
                            //DiscountAmount = Convert.ToInt32(discountAmountBox.Text),
                            //QuantityInStock = Convert.ToInt32(quantityInStockBox.Text),
                            //Status = statusBox.Text,
                            //MaxDiscount = Convert.ToDecimal(maxDiscountBox.Text),
                            //Supplier = supplierBox.Text,
                            //Unit = unitBox.Text
                        };

                        
                        if (String.IsNullOrEmpty(CurrentProduct.Gosnomer))
                        {
                            MessageBox.Show("Укажите госномер");
                            return;
                        }

                        if (String.IsNullOrEmpty(CurrentProduct.Vladelec))
                        {
                            MessageBox.Show("Укажите владельца");
                            return;
                        }


                        /*
                        if (CurrentProduct.QuantityInStock < 0)
                        {
                            MessageBox.Show("Количество товаров на складе не может быть меньше нуля!");
                            return;
                        }*/

                        db.Car.Add(CurrentProduct);


                        // если не было выбрано фото
                        /*
                        if (String.IsNullOrEmpty(newImage))
                        {
                            CurrentProduct.Photo = "picture.png";
                            BitmapImage image = new BitmapImage(new Uri(CurrentProduct.ImagePath));
                            image.CacheOption = BitmapCacheOption.OnLoad;
                            CurrentProduct.ImagePath = image.UriSource.ToString();
                        }
                        else // если выбрано фото
                        {
                            string newRelativePath = $"{System.DateTime.Now.ToString("HHmmss")}_{newImage}";
                            product.Photo = newRelativePath;

                            File.Copy(newImagePath, System.IO.Path.Combine(Environment.CurrentDirectory, $"images/{newRelativePath}"));

                            BitmapImage image = new BitmapImage(new Uri(CurrentProduct.ImagePath));
                            image.CacheOption = BitmapCacheOption.OnLoad;
                            CurrentProduct.ImagePath = image.UriSource.ToString();
                        }*/

                        db.SaveChanges();

                        MessageBox.Show("Автомобиль успешно добавлен!");
                        // обновляем свойство
                        CountProducts = $"Количество: {CurrentProducts.Count()} из {db.Car.ToList().Count}";
                        CurrentProducts = db.Car.ToList();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.InnerException.ToString());
                    }

                }
                else
                {
                    /*
                    if (CurrentProduct.Cost < 0)
                    {
                        MessageBox.Show("Цена должна быть положительной!");
                        return;
                    }

                    if (CurrentProduct.QuantityInStock < 0)
                    {
                        MessageBox.Show("Количество товаров на складе не может быть меньше нуля!");
                        return;
                    }


                    // если выбрано новое фото
                    if (newImage != null)
                    {
                        string newRelativePath = $"{System.DateTime.Now.ToString("HHmmss")}_{newImage}";
                        CurrentProduct.Photo = newRelativePath;
                        MessageBox.Show($"Новое фото: {CurrentProduct.Photo} присвоено!");
                        File.Copy(newImagePath, System.IO.Path.Combine(Environment.CurrentDirectory, $"images/{CurrentProduct.Photo}"));
                        newImage = null;
                    }


                    // если есть старое фото, то пытаемся его удалить

                    if (!string.IsNullOrEmpty(oldImage))
                    {
                        try
                        {
                            File.Delete(oldImage);
                            MessageBox.Show($"Старое фото: {oldImage} удалено!");
                            oldImage = null;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString());
                        }
                    }*/


                    try
                    {
                        db.Car.Update(CurrentProduct);
                        db.SaveChanges();
                        MessageBox.Show("Автомобиль успешно обновлен!");

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }

                }
            }
        }
        #endregion

        #region Команда AddImageCommand
        public ICommand AddImageCommand { get; set; }

        private bool CanAddImageCommandExecute(object p)
        {
            return true;
        }

        private void OnAddImageCommandExecuted(object p)
        {
            Stream myStream;

            if (CurrentProduct.Id != null)
            {
                //oldImage = System.IO.Path.Combine(Environment.CurrentDirectory, $"images/{CurrentProduct.Photo}");
                oldImage = null;
            }
            else
            {
                oldImage = null;
            }

            // проверяем, есть ли изображение у товара и запоминаем путь к изображению сейчас

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            if (dlg.ShowDialog() == true)
            {
                if ((myStream = dlg.OpenFile()) != null)
                {
                    dlg.DefaultExt = ".png";
                    dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
                    dlg.Title = "Open Image";
                    dlg.InitialDirectory = "./";

                    // Предпросмотр изображения
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    image.UriSource = new Uri(dlg.FileName);

                    //MessageBox.Show($"Изображения: {image.Width} на {image.Height} пикселей. Размер будет приведен к 200 на 300 пикселей! ");
                    //image.DecodePixelWidth = 300;
                    //image.DecodePixelHeight = 200;
                    //imageBoxPath.Source = image;

                    //CurrentProduct.Photo = dlg.SafeFileName;
                    //MessageBox.Show(CurrentProduct.Photo);
                    //CurrentProduct.ImagePath = image.UriSource.ToString();
                    //MessageBox.Show(CurrentProduct.ImagePath);


                    image.EndInit();

                    try
                    {

                        newImage = dlg.SafeFileName;
                        //MessageBox.Show($"newImage: {newImage}");
                        newImagePath = dlg.FileName;
                        //MessageBox.Show($"newImagePath: {newImagePath}");
                        //MessageBox.Show(File.Exists(newImagePath).ToString());

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                myStream.Dispose();
            }
        }
        #endregion

        #region Команда AddProductCommand
        public ICommand AddProductCommand { get; set; }

        private bool CanAddProductCommandExecute(object p)
        {
            return true;
        }

        private void OnAddProductCommandExecuted(object p)
        {
            AddProductWindow addProductWindow = new AddProductWindow();
            addProductWindow.DataContext = this;
            IdPanelVisibility = Visibility.Collapsed;
            Title = "Добавление машины";
            CurrentProduct = new Cars();
            addProductWindow.ShowDialog();
        }
        #endregion

        #region Команда AddCheckCommand
        public ICommand AddCheckCommand { get; set; }

        private bool CanAddCheckCommandExecute(object p)
        {
            return true;
        }

        private void OnAddCheckCommandExecuted(object p)
        {
            PrintCheckWindows addCheckWindow = new PrintCheckWindows();
            addCheckWindow.DataContext = this;
            //IdPanelVisibility = Visibility.Collapsed;
            //Title = "Добавление чека";
            //CurrentProduct = new Cars();
            addCheckWindow.ShowDialog();
        }
        #endregion

        #region Команда PrintCkeck
        public ICommand PrintCkeck { get; set; }

        private bool CanPrintCheckExecute(object p)
        {
            return true;
        }

        private FlowDocument CreateFlowDocument()
        {
            // Create a FlowDocument  
            FlowDocument doc = new FlowDocument();
            // Create a Section  
            Section sec = new Section();
            // Create first Paragraph  
            Paragraph p1 = new Paragraph();
            // Create and add a new Bold, Italic and Underline  
            Bold bld = new Bold();
            bld.Inlines.Add(new Run("Феррари - диагностика правого крыла. 21.03.2023. Стоимость 4000 рублей"));
            Italic italicBld = new Italic();
            italicBld.Inlines.Add(bld);
            Underline underlineItalicBld = new Underline();
            underlineItalicBld.Inlines.Add(italicBld);
            // Add Bold, Italic, Underline to Paragraph  
            p1.Inlines.Add(underlineItalicBld);
            // Add Paragraph to Section  
            sec.Blocks.Add(p1);
            // Add Section to FlowDocument  
            doc.Blocks.Add(sec);
            return doc;
        }

        private void OnPrintCheckExecuted(object p)
        {
            // Create a PrintDialog  
            PrintDialog printDlg = new PrintDialog();
            // Create a FlowDocument dynamically.  
            FlowDocument doc = CreateFlowDocument();
            doc.Name = "FlowDoc";
            // Create IDocumentPaginatorSource from FlowDocument  
            IDocumentPaginatorSource idpSource = doc;
            // Call PrintDocument method to send document to printer  
            printDlg.PrintDocument(idpSource.DocumentPaginator, "Печать чека.");

        }
        #endregion

        #region Команда ClearCommand
        public ICommand ClearCommand { get; set; }

        private bool CanClearCommandExecute(object p)
        {
            return true;
        }

        private void OnClearCommandExecuted(object p)
        {
            FilterSelectedIndex = -1;
            SortSelectedIndex= -1;
            SearchText = "";
        }
        #endregion

        #region Команда UpdateProductCommand
        public ICommand UpdateProductCommand { get; set; }

        private bool CanUpdateProductCommandExecute(object p)
        {
            return true;
        }

        private void OnUpdateProductCommandExecuted(object p)
        {

            using (CarSTOContext db = new CarSTOContext())
            {

                 CurrentProducts = db.Car.ToList();
               /*
                //Сортировка
                if (SortSelectedIndex != -1)
                {
                    if (SortSelectedValue.Contains("По убыванию цены"))
                    {
                        CurrentProducts = CurrentProducts.OrderByDescending(u => u.Cost);

                    }

                    if (SortSelectedValue.Contains("По возрастанию цены"))
                    {
                        CurrentProducts = CurrentProducts.OrderBy(u => u.Cost);

                    }
                }*/

                /*
                // Фильтрация
                if (FilterSelectedIndex != -1)
                {

                    if (db.Car.Select(u => u.Manufacturer).Distinct().ToList().Contains(FilterSelectedValue))
                    {
                        CurrentProducts = CurrentProducts.Where(u => u.Manufacturer == FilterSelectedValue.ToString()).ToList();
                    }
                    else
                    {
                        CurrentProducts = CurrentProducts.ToList();
                    }
                }*/

                //Поиск

                if (SearchText != null)
                {
                    CurrentProducts = CurrentProducts.Where(u => u.Vladelec.Contains(SearchText) ||  u.Gosnomer.Contains(SearchText)).ToList();

                }

                CountProducts = $"Количество: {CurrentProducts.Count()} из {db.Car.ToList().Count}";
            }
            
            }
        #endregion



        #region Свойство Car

        public ObservableCollection<Cars> Car { get; set; }
        #endregion

        #region Свойство CurrentProducts

        private IEnumerable<Cars> _currentProducts;
        public IEnumerable<Cars> CurrentProducts
        {
            get { return _currentProducts; }

            set
            {
                Set(ref _currentProducts, value);
            }

        }

        #endregion

        #region Свойство SortSelectedIndex
        private int _sortSelectedIndex;
        public int SortSelectedIndex
        {
            get => _sortSelectedIndex;
            set => Set(ref _sortSelectedIndex, value);
        }
        #endregion

        #region Свойство SortSelectedValue

        public string SortSelectedValue { get; set; }
        #endregion

        #region Свойство SortSource

        public List<string> SortSource { get; set; }
        #endregion

        #region Свойство FilterSelectedIndex
   
        private int _filterSelectedIndex;
        public int FilterSelectedIndex
        {
            get => _filterSelectedIndex;
            set => Set(ref _filterSelectedIndex, value);
        }
        #endregion

        #region Свойство FilterSelectedValue

        public string FilterSelectedValue { get; set; }
        #endregion

        #region Свойство FilterSource

        public List<string> FilterSource { get; set; }
        #endregion

        #region Свойство CurrentRole
        private string _currentRole;
        public string CurrentRole
        {
            get { return _currentRole; }

            set
            {
                //countUser = value;
                //OnPropertyChanged("CountUser");

                Set(ref _currentRole, value);
            }

        }
        #endregion

        #region Свойство SearchText

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => Set(ref _searchText, value);
        }
        #endregion

        #region Свойство CountProducts

        private string _countProducts;
        public string CountProducts
        {
            get => _countProducts;
            set => Set(ref _countProducts, value);
        }
        #endregion

        #region Свойство CurrentProduct

        private Cars _currentProduct;
        public Cars CurrentProduct
        {
            get => _currentProduct;
            set => Set(ref _currentProduct, value);
        }
        #endregion

        #region Свойство Categories

        private List<string> _categories;
        public List<string> Categories
        {
            get => _categories;
            set => Set(ref _categories, value);
        }
        #endregion

        #region Свойство AdminPanelVisibility
        private Visibility _adminPanelVisibility;
        public Visibility AdminPanelVisibility
        {
            get { return _adminPanelVisibility; }

            set
            {
                Set(ref _adminPanelVisibility, value);
            }

        }
        #endregion

        #region Свойство IdPanelVisibility
        private Visibility _idPanelVisibility;
        public Visibility IdPanelVisibility
        {
            get { return _idPanelVisibility; }

            set
            {
                Set(ref _idPanelVisibility, value);
            }

        }
        #endregion

        #region Свойство Title
        private string _title;
        public string Title
        {
            get { return _title; }

            set
            {
                Set(ref _title, value);
            }

        }
        #endregion




        /// <summary>
        /// Проверка возможности удаления продукта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private bool CanDeleteProduct(Cars product)
        {
            using (CarSTOContext db = new CarSTOContext())
            {
                /*
                // провека наличи оригинального товара в заказе
                OrderProduct position = db.OrderProducts.Where(o => o.ProductId == product.Id).FirstOrDefault() as OrderProduct;

                if (position is not null)
                {
                    MessageBox.Show($"Товар: {product.Vladelec} присутствует в товарной позиции заказа {position.OrderId}. \n Товар нельзя удалить!");
                    return false;
                }


                
                    // найдем все связанные товары с данным товаром
                    List<RelatedProduct> rp = db.RelatedProducts.Where(p => p.ProductId == product.Id).ToList();

                    // найдем, существуют ли товарные позиции из нашего списка связанных товаров и самого товара в заказах
                    foreach (RelatedProduct r in rp)
                    {
                        OrderProduct order = db.OrderProducts.Where(o => o.ProductId == r.RelatedProdutId).FirstOrDefault() as OrderProduct;
                        if (order is not null)
                        {
                            Cars p = db.Car.Where(p => p.Id == r.RelatedProdutId).FirstOrDefault() as Cars;
                            MessageBox.Show($"Товар {p.Vladelec} связан с товаром {product.Vladelec} присутствует в товарной позиции заказа {order.OrderId}. \n Товары нельзя удалить!");
                            return false;
                        }
                    }
                */

                    
                

                return true;
            }
        }

        public CarViewModel(User user)
        {
            CurrentProduct = new Cars();

            if (user != null)
            {
                CurrentRole = $"{user.RoleNavigation.Name}: {user.Surname} {user.Name} {user.Patronymic}";
            }
            else
            {
                CurrentRole = "Гость";
            }

            Car = new ObservableCollection<Cars>();
            using (CarSTOContext db = new CarSTOContext())
            {
                List<Cars> temp = db.Car.ToList();

                foreach (var item in temp)
                {
                    Car.Add(item);
                }

                CurrentProducts = db.Car.ToList();

                //List<string> filterList = db.Car.Select(p => p.Manufacturer).Distinct().ToList();
                //filterList.Insert(0, "Все производители");
                //FilterSource = filterList;

                SortSource = new List<string>() { "По возрастанию цены", "По убыванию цены" };


                CountProducts = $"Количество: {CurrentProducts.Count()} из {db.Car.ToList().Count}";

                //Categories = db.Car.Select(p => p.Category).Distinct().ToList();


                if (user == null || user.RoleNavigation.Name != "Администратор")
                {
                   AdminPanelVisibility = Visibility.Collapsed;
                }
                
                

            }




            ExitCommand = new LambdaCommand(OnExitCommandExecuted,CanExitCommandExecute);
            UpdateProductCommand = new LambdaCommand(OnUpdateProductCommandExecuted, CanUpdateProductCommandExecute);
            ClearCommand = new LambdaCommand(OnClearCommandExecuted, CanClearCommandExecute);
            AddProductCommand = new LambdaCommand(OnAddProductCommandExecuted, CanAddProductCommandExecute);
            AddCheckCommand = new LambdaCommand(OnAddCheckCommandExecuted, CanAddCheckCommandExecute); //PrintCkeck = new LambdaCommand(OnAddCheckCommandExecuted, CanAddCheckCommandExecute);
            PrintCkeck = new LambdaCommand(OnPrintCheckExecuted, CanPrintCheckExecute);
            AddImageCommand = new LambdaCommand(OnAddImageCommandExecuted, CanAddImageCommandExecute);
            SaveProductCommand = new LambdaCommand(OnSaveProductCommandExecuted, CanSaveProductCommandExecute);
            EditProductCommand = new LambdaCommand(OnEditProductCommandExecuted, CanEditProductCommandExecute);
            DeleteCommand = new LambdaCommand(OnDeleteCommandExecuted, CanDeleteCommandExecute);
        }
    }
}

namespace WPFTest.UI.Chapter3
{
    /// <summary>
    /// ProCon.xaml 的交互逻辑
    /// </summary>
    public partial class ProCon : ChildPage
    {
        public Semaphore fill_num;  //表示buffer中已经占用资源数
        public Semaphore empty_num;  //表示buffer中还空闲资源数
        public Mutex mutex;  //互斥地访问共享资源

        Thread[] producers;
        Thread[] consumers;

        //一个生产者生产产品的时间
        const int Producetimes = 1000;
        //buffer所能容纳的最大产品数量
        const int SemaphoreNum = 20;
        bool isStart;

        public ProCon()
        {
            InitializeComponent();
            isStart = false;
        }

        public ProCon(MainWindow parent)
        {
            InitializeComponent();
            this.parentWindow = parent;

        }

        private void ChildPage_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void showComment(String comment)
        {
            if (MyStringUtil.isEmpty(comment))
            {
                listBox_new.Items.Add("");
                return;
            }

            listBox_new.Items.Add(comment);
        }
        
        //定义回调
        private delegate void Update(int number); 
        public void update(int number)
        {
            if (!listBox_new.Dispatcher.CheckAccess())
            {
                //声明并实例化回调
                Update m = Updatelistbox;
                //使用回调
                listBox_new.Dispatcher.Invoke(m, number);
            }
            else
            {
                showComment(number.ToString());
            }
        }

        private void btn_begin_Click(object sender, RoutedEventArgs e)
        {
            listBox_new.Items.Clear();
            if (!isStart)
            {
                mutex = new Mutex();
                isStart = true;
                try
                {
                    fill_num = new Semaphore(0, SemaphoreNum);
                    empty_num = new Semaphore(SemaphoreNum, SemaphoreNum);
                    
                    consumers = new Thread[int.Parse(Consumer_num.Text)];
                    producers = new Thread[int.Parse(Producer_num.Text)];
                }
                catch (Exception)
                {
                    showComment("出现错误！");
                    return;
                }

                //创建生产者和消费者线程
                for (int i = 0; i < int.Parse(Producer_num.Text); i++)
                {
                    producers[i] = new Thread(() => { Produce(i); });
                    producers[i].Start();
                }

                for (int i = 0; i < int.Parse(Consumer_num.Text); i++)
                {
                    consumers[i] = new Thread(() => { Consume(i); });
                    consumers[i].Start();
                }
                showComment("开始执行生产和消费");
            }
            else
            {
                showComment("需要先停止，然后才能开始！");
            }
        }

        //生产者的生产函数
        public void Produce(int number)
        {
            while (true)
            {
                empty_num.WaitOne();
                mutex.WaitOne();
                update(number);
                mutex.ReleaseMutex();
                fill_num.Release();
                Thread.Sleep(Producetimes);
            }

        }

        //消费者的消费函数
        public void Consume(int number)
        {
            while (true)
            {
                fill_num.WaitOne();
                mutex.WaitOne();
                update(-1 * number);
                mutex.ReleaseMutex();
                empty_num.Release();
            }
        }


        //更新listbox中Show的信息
        //number<0说明是消费者调用，number>0说明是生产者调用
        public void Updatelistbox(int number)
        {
            if (number < 0)
            {
                showComment("消费者" + number * -1 + "消费了一个产品。\n");
            }
            else
            {
                showComment("生产者" + number + "生产了一个产品。\n");
            }
        }

        //关闭所有线程
        public void CloseAll()
        {
            for (int i = 0; i < producers.Length; i++)
            {
                producers[i].Abort();
            }
            for (int i = 0; i < consumers.Length; i++)
            {
                consumers[i].Abort();
            }
        }

        //窗口关闭则关闭所有线程
        public void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CloseAll();
        }

        private void btn_end_Click(object sender, RoutedEventArgs e)
        {
            if (isStart)
            {
                CloseAll();
                isStart = false;
            }
            else
            {
                showComment("您还未开始，请先输入生产者消费者数量，点击开始按钮！");
            }
        }

        private void btn_clean_Click(object sender, RoutedEventArgs e)
        {
            Producer_num.Text = "";
            Consumer_num.Text = "";
            listBox_new.Items.Clear();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cortex.UI
{
    public partial class FileSelect : Component
    {
        public FileSelect()
        {
            InitializeComponent();
        }

        public FileSelect(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}

﻿//using AmazonClone.Model;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

using AmazonClone.Model;

namespace AmazonClone.Test.Model
{
    public class CustomerShould
    {
        [Fact]
        public void ShouldHaveAddress() 
        {
            Customer sut = new Customer("address");
            Assert.NotNull(sut.Address);
        }
    }
}


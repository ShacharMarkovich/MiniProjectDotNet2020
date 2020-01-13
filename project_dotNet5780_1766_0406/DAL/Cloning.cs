using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    using System.Reflection;
    using BE;
    /// <summary>
    /// static class for deep copy.
    /// Copy source fields to new instance
    /// </summary>
    /// <typeparam name="T">The type argument must have a public parameterless c'tor.</typeparam>
    /// <returns>returns a new instance of T with the same values as source</returns>
    public static class Cloning
    {
        /// <summary>
        /// template function for deep copy.
        /// Copy source fields to new instance
        /// </summary>
        /// <typeparam name="T">The type argument must have a public parameterless c'tor.</typeparam>
        /// <returns>returns a new instance of T with the same values as source</returns>
        public static T clone<T>(this T source) where T : new()
        {
            // create new type and save his type
            T target = new T();
            Type type = typeof(T);

            // copy all properties from source to target
            foreach (PropertyInfo sourceProperty in type.GetProperties())
            {
                PropertyInfo targetProperty = type.GetProperty(sourceProperty.Name); // get property name
                targetProperty.SetValue(target, sourceProperty.GetValue(source, null), null);
            }

            // copy all field (the data) from source to target
            foreach (FieldInfo sourceField in type.GetFields())
            {
                FieldInfo targetField = type.GetField(sourceField.Name); // get field name
                targetField.SetValue(target, sourceField.GetValue(source));
            }

            // return clonning object
            return target;
        }
        /*
        public static BankBranch Clone(this BankBranch source)
        {
            BankBranch newBank = new BankBranch();
            newBank.BankNumber = source.bankNumber;
            newBank.BankName = source.BankName;
            newBank.BranchNumber = source.BranchNumber;
            newBank.BranchAddress = source.BranchAddress;
            newBank.BranchCity = source.BranchCity;
            return newBank;
        }

        public static GuestRequest Clone(this GuestRequest source)
        {
            GuestRequest newGR = new GuestRequest();
            newGR.GuestRequestKey = source.GuestRequestKey;
            newGR.PrivateName = source.PrivateName;
            newGR.FamilyName = source.FamilyName;
            newGR.Email = source.Email;
            newGR.Stat = source.Stat;
            newGR.RegistrationDate = source.RegistrationDate;
            newGR.EntryDate = source.EntryDate;
            newGR.ReleaseDate = source.ReleaseDate;
            newGR.Area = source.Area;
            newGR.type = source.type;
            newGR.Adults = source.Adults;
            newGR.Children = source.Children;
            newGR.Pool = source.Pool;
            newGR.Jecuzzi = source.Jecuzzi;
            newGR.Garden = source.Garden;
            newGR.ChildrenAttractions = source.ChildrenAttractions;

            return newGR;
        }

        public static Host Clone(this Host source)
        {
            Host newHost = new Host();
            newHost.HostKey = source.HostKey;
            newHost.PrivateName = source.PrivateName;
            newHost.FamilyName = source.FamilyName;
            newHost.PhoneNumber = source.PhoneNumber;
            newHost.Email = source.Email;
            newHost.BankBranchDetails = source.BankBranchDetails.Clone();
            newHost.BankAccountNumber = source.BankAccountNumber;
            newHost.Balance = source.Balance;
            newHost.CollectionClearance = source.CollectionClearance;

            return newHost;
        }

        public static HostingUnit Clone(this HostingUnit source)
        {
            HostingUnit newUnit = new HostingUnit();
            newUnit.HostingUnitKey = source.HostingUnitKey;
            newUnit.Owner = source.Owner.Clone();
            newUnit.Area = source.Area;
            newUnit.type = source.type;
            newUnit.HostingUnitName = source.HostingUnitName;
            newUnit.Diary = source.Diary;

            return newUnit;
        }

        public static Order Clone(this Order source)
        {
            Order newOrder = new Order();
            newOrder.HostingUnitKey = source.HostingUnitKey;
            newOrder.GuestRequestKey = source.GuestRequestKey;
            newOrder.OrderKey = source.OrderKey;
            newOrder.Status = source.Status;
            newOrder.CreateDate = source.CreateDate;
            newOrder.OrderDate = source.OrderDate;

            return newOrder;
        }*/
    }
}

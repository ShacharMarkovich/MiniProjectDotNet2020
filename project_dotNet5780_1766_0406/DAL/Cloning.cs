using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    using System.Reflection;
    public static class Cloning
    {
        /// <summary>
        /// template function for deep copy.
        /// Copy source fields to 
        /// </summary>
        /// <typeparam name="T">The type argument must have a public parameterless c'tor.</typeparam>
        /// <returns>returns a new instance of T with the same values as source</returns>
        public static T Clone<T>(this T source) where T : new()
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
        public static BE.BankBranch Claone(this BE.BankBranch branch)
        {
            BE.BankBranch newB = new BE.BankBranch();
            newB.BankName = branch.BankName;
            newB.BankNumber = branch.BankNumber;
            newB.BranchAddress = branch.BranchAddress;
            newB.BranchCity = branch.BranchCity;
            newB.BranchNumber = branch.BranchNumber;
            return newB;
        }
        public static BE.GuestRequest Claone(this BE.GuestRequest gReq)
        {
            BE.GuestRequest newGReq = new BE.GuestRequest();
            newGReq.GuestRequestKey = gReq.GuestRequestKey;
            newGReq.PrivateName = gReq.PrivateName;
            newGReq.FamilyName = gReq.FamilyName;
            newGReq.Email = gReq.Email;
            newGReq.Stat = gReq.Stat;
            newGReq.RegistrationDate = gReq.RegistrationDate;
            newGReq.EntryDate = gReq.EntryDate;
            newGReq.ReleaseDate = gReq.RegistrationDate;
            newGReq.Area = gReq.Area;
            newGReq.type = gReq.type;
            newGReq.Adults = gReq.Adults;
            newGReq.Children = gReq.Children;
            newGReq.Pool = gReq.Pool;
            newGReq.Jecuzzi = gReq.Jecuzzi;
            newGReq.Garden = gReq.Garden;
            newGReq.ChildrenAttractions = gReq.ChildrenAttractions;

            return newGReq;
        }
        public static BE.Host Claone(this BE.Host host)
        {
            BE.Host newH = new BE.Host();

            foreach (System.Reflection.PropertyInfo sourceProperty in typeof(BE.Host).GetProperties())
            {
                System.Reflection.PropertyInfo targetProperty = typeof(BE.Host).GetProperty(sourceProperty.Name);
                targetProperty.SetValue(newH, sourceProperty.GetValue(host, null), null);
            }
            foreach (System.Reflection.FieldInfo sourceField in typeof(BE.Host).GetFields())
            {
                var targetField = typeof(BE.Host).GetField(sourceField.Name);
                targetField.SetValue(newH, sourceField.GetValue(host));
            }
            return newH;
        }
        public static BE.HostingUnit Claone(this BE.HostingUnit unit)
        {
            return null;
        }
        public static BE.Order Claone(this BE.Order order)
        {
            return null;
        }
        */
    }
}

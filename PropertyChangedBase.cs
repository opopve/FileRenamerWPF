﻿using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Tools
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyChangedAlsoAttribute : Attribute
    {
        public string[] PropertiesNames;

        public PropertyChangedAlsoAttribute(params string[] propertiesNames) {
            PropertiesNames = propertiesNames;
        }
    }

    public abstract class PropertyChangedBase : INotifyPropertyChanged
    {
        public virtual void FirePropertyChanged() {
            if(PropertyChanged != null)
                PropertyChanged.Invoke(null, null);
        }

        public virtual event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string propertyName = "") {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null) {
                var att = this.GetType().GetProperty(propertyName).GetCustomAttributes(typeof(PropertyChangedAlsoAttribute), true);
                handler(this, new PropertyChangedEventArgs(propertyName));
                if(att.Length > 0) {
                    foreach(string propName in (att[0] as PropertyChangedAlsoAttribute).PropertiesNames)
                        handler(this, new PropertyChangedEventArgs(propName));
                }
            }
        }

        /// <summary>
        /// Устанавливаем поле с вызовом события обновления, при вызове внутри метода set указывать имя свойства не обязательно.
        /// </summary>
        protected bool SetField<T>(ref T field, T value, [CallerMemberName]string propertyName = "") {
                if(EqualityComparer<T>.Default.Equals(field, value))
                    return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        //No string implementation
        protected void OnPropertyChanged<T>(Expression<Func<T>> selectorExpression) {
            OnPropertyChanged(GetPropertyName(selectorExpression));
        }

        protected bool SetField<T>(ref T field, T value, Expression<Func<T>> selectorExpression) {
                if(EqualityComparer<T>.Default.Equals(field, value))
                    return false;
            field = value;
            OnPropertyChanged(selectorExpression);
            return true;
        }

        protected string GetPropertyName<T>(Expression<Func<T>> selectorExpression) {
            if(selectorExpression == null)
                throw new ArgumentNullException("selectorExpression");
            MemberExpression body = selectorExpression.Body as MemberExpression;
            if(body == null)
                throw new ArgumentException("The body must be a member expression");
            return body.Member.Name;
        }
    }
}
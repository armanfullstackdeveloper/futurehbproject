using System;
using System.Collections.Generic;
using System.Reflection;
using DataModel.Entities;
using DataModel.Models.ViewModel;
using Newtonsoft.Json.Linq;
using NHibernate;
using NHibernate.Criterion;

namespace BusinessLogic.Helpers
{
    public class GenericRepository<T, TPkT> : DapperConfiguration, IRepository<T, TPkT> where T : EntityBase<T>, new()
    {
        public TPkT Insert(T pModel)
        {
            TPkT generatedCode;
            try
            {
                using (ISession session = NHibernateConfiguration.OpenSession())
                using (ITransaction transaction = session.BeginTransaction())
                {
                    generatedCode = (TPkT)session.Save(pModel);
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("duplicate key"))
                {
                    throw new MyExceptionHandler("امکان درج داده تکراری نمی باشد", ex, JObject.FromObject(pModel).ToString());
                }
                throw new MyExceptionHandler(ex.ToString(), ex, JObject.FromObject(pModel).ToString());
            }
            return generatedCode;
        }

        public TPkT InsertWhitOutCommitTransaction(T pModel, ISession pSession)
        {
            TPkT generatedCode;
            try
            {
                generatedCode = (TPkT)pSession.Save(pModel);
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("Cannot insert duplicate key"))
                {
                    throw new MyExceptionHandler("امکان درج داده تکراری نمی باشد", ex, JObject.FromObject(pModel).ToString());
                }
                throw new MyExceptionHandler(ex.ToString(), ex, JObject.FromObject(pModel).ToString());
            }
            return generatedCode;
        }

        public bool Update(T pModel)
        {
            try
            {
                using (ISession session = NHibernateConfiguration.OpenSession())
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(pModel);
                    transaction.Commit();
                }
                return true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("duplicate key"))
                {
                    throw new MyExceptionHandler("امکان درج داده تکراری نمی باشد", ex, JObject.FromObject(pModel).ToString());
                }
                throw new MyExceptionHandler(ex.ToString(), ex, JObject.FromObject(pModel).ToString());
            }
        }

        public bool UpdateWhitOutCommitTransaction(T pModel, ISession pSession)
        {
            try
            {
                pSession.Update(pModel);
                return true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("Cannot insert duplicate key"))
                {
                    throw new MyExceptionHandler("امکان درج داده تکراری نمی باشد", ex, JObject.FromObject(pModel).ToString());
                }
                throw new MyExceptionHandler(ex.ToString(), ex, JObject.FromObject(pModel).ToString());
            }
        }

        public bool Delete(TPkT code)
        {
            try
            {
                using (ISession session = NHibernateConfiguration.OpenSession())
                using (ITransaction transaction = session.BeginTransaction())
                {

                    session.CreateSQLQuery("delete from [" + typeof(T).Name + "] where id = :id").SetParameter("id", code).ExecuteUpdate();
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("conflicted with the REFERENCE constraint"))
                {
                    throw new MyExceptionHandler("بعلت استفاده شدن در بخش های دیگر امکان حذف نمی باشد", ex, code.ToString());
                }
                throw new MyExceptionHandler(ex.ToString(), ex, code.ToString());
            }
            return true;
        }

        public bool DeleteWhitOutCommitTransaction(TPkT code, ISession pSession)
        {
            try
            {

                String query = "delete from [" + typeof(T).Name + "] where id= :id";
                pSession.CreateSQLQuery(query).SetParameter("id", code).ExecuteUpdate();

            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("refre"))
                {
                    throw new MyExceptionHandler("بعلت استفاده شدن در بخش های دیگر امکان حذف نمی باشد", ex, code.ToString());
                }
                throw new MyExceptionHandler(ex.ToString(), ex, code.ToString());
            }
            return true;
        }

        public IList<T> SelectAll()
        {
            try
            {

                using (ISession session = NHibernateConfiguration.OpenSession())
                {
                    IQuery query = session.CreateQuery("From " + typeof(T).Name);
                    IList<T> list = query.List<T>();
                    foreach (T model in list)
                    {
                        session.Evict(model);
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, string.Empty);
            }
        }

        public IList<T> SelectAllWhitOutCommitTransaction(ISession pSession)
        {
            try
            {
                IQuery query = pSession.CreateQuery("From " + typeof(T).Name);
                IList<T> list = query.List<T>();
                foreach (T model in list)
                {
                    pSession.Evict(model);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, string.Empty);
            }
        }

        public IList<T> SelectAllWithTopCount(int topCount)
        {
            try
            {
                using (ISession session = NHibernateConfiguration.OpenSession())
                {
                    IQuery query = session.CreateQuery("From " + typeof(T).Name);
                    IList<T> list = query.SetFirstResult(0).SetMaxResults(100).List<T>();

                    foreach (T model in list)
                    {
                        session.Evict(model);
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, topCount.ToString());
            }
        }

        public IList<T> SelectAllWithPageSize(int pageNo,int pageSize)
        {
            pageNo -= 1;
            try
            {
                using (ISession session = NHibernateConfiguration.OpenSession())
                {
                    IQuery query = session.CreateQuery("From " + typeof(T).Name);
                    IList<T> list = query.SetFirstResult(pageNo * pageSize).SetMaxResults(pageSize).List<T>();

                    foreach (T model in list)
                    {
                        session.Evict(model);
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                {
                    new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => pageNo),
                        Value = pageNo.ToString()
                    },
                                        new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => pageSize),
                        Value = pageSize.ToString()
                    },
                };
                throw new MyExceptionHandler(ex.ToString(), ex, JArray.FromObject(lst).ToString());
            }
        }

        public T SelectOne(TPkT code)
        {
            try
            {
                using (ISession session = NHibernateConfiguration.OpenSession())
                {
                    IQuery query = session.CreateQuery("from " + typeof(T).Name + " where Id= :x ");
                    query.SetString("x", code.ToString());
                    T s = (T)query.UniqueResult();
                    return s;
                }
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, code.ToString());
            }
        }

        public T SelectOneWhitOutCommitTransaction(TPkT code, ISession pSession)
        {
            try
            {

                IQuery query = pSession.CreateQuery("from " + typeof(T).Name + " where Id= :x ");
                query.SetString("x", code.ToString());
                T s = (T)query.UniqueResult();
                return s;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, code.ToString());
            }
        }

        /// <summary>
        /// انتخاب رکورد براساس مقادیر فیلدهای پر شده مدل
        /// </summary>
        /// <param name="pModel">مدلی که صفر،یک یا چند فیلد آن پر می باشد</param>
        /// <returns></returns>
        public IList<T> FindObject(T pModel)
        {
            try
            {
                Type t = typeof(T);
                using (ISession session = NHibernateConfiguration.OpenSession())
                {
                    Example example = Example.Create(pModel).IgnoreCase().EnableLike(MatchMode.Anywhere);
                    ICriteria iCriteria = session.CreateCriteria(typeof(T)).Add(example);
                    const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
                    foreach (var item in t.GetProperties(bindingFlags))
                    {
                        if (
                            System.String.Compare(item.PropertyType.Assembly.GetName().Name, "LegalMelli.DataModel",
                                System.StringComparison.Ordinal) != 0) continue;
                        object obj = pModel.GetType().GetProperty(item.Name).GetValue(pModel, null);
                        if (obj == null) continue;
                        Example example1 = Example.Create(obj).IgnoreCase().EnableLike(MatchMode.Anywhere);
                        iCriteria.CreateCriteria(item.Name).Add(example1);
                        object id = obj.GetType().GetProperty("ID").GetValue(obj, null);
                        if (id != null && !id.Equals(0))
                        {
                            iCriteria.Add(Expression.Eq(item.Name + ".ID", id));
                        }
                    }
                    return iCriteria.List<T>();
                }
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, JObject.FromObject(pModel).ToString());
            }
        }

        /// <summary>
        /// انتخاب رکورد براساس مقادیر فیلدهای پر شده مدل
        ///  بدون درنظر گرفتن فیلدهایی که مقدار پیش فرض صفر دارند
        /// </summary>
        /// <param name="pModel">مدلی که صفر،یک یا چند فیلد آن پر می باشد</param>
        /// <returns></returns>
        public IList<T> FindObject_ExcludeZeroes(T pModel)
        {
            try
            {
                Type t = typeof(T);
                using (ISession session = NHibernateConfiguration.OpenSession())
                {
                    Example example = Example.Create(pModel).IgnoreCase().ExcludeZeroes();
                    ICriteria iCriteria = session.CreateCriteria(typeof(T)).Add(example);
                    const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
                    foreach (var item in t.GetProperties(bindingFlags))
                    {
                        if (System.String.Compare(item.PropertyType.Assembly.GetName().Name, "DataModel", System.StringComparison.Ordinal) == 0)
                        {
                            object obj = pModel.GetType().GetProperty(item.Name).GetValue(pModel, null);
                            if (obj == null) continue;
                            Example example1 = Example.Create(obj).IgnoreCase().ExcludeZeroes();
                            iCriteria.CreateCriteria(item.Name).Add(example1);
                            object id = obj.GetType().GetProperty("ID").GetValue(obj, null);
                            if (id != null && System.String.Compare(id.ToString(), "0", System.StringComparison.Ordinal) != 0)
                            {
                                iCriteria.Add(Expression.Eq(item.Name + ".ID", id));
                            }
                        }
                    }
                    return iCriteria.List<T>();
                }
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, JObject.FromObject(pModel).ToString());
            }
        }

        /// <summary>
        /// انتخاب رکورد براساس مقادیر فیلدهای پر شده مدل بدون درنظر گرفتن
        ///  فیلدهایی که مقدار پیش فرض صفر دارند و براساس تعداد رکورد شماره صفحه
        /// </summary>
        /// <param name="pModel">مدلی که صفر،یک یا چند فیلد آن پر می باشد</param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<T> FindObject_ExcludeZeroes_WithPageSize(T pModel, int pageNo, short pageSize)
        {
            pageNo -= 1;
            try
            {
                Type t = typeof(T);
                using (ISession session = NHibernateConfiguration.OpenSession())
                {
                    Example example = Example.Create(pModel).IgnoreCase().ExcludeZeroes().EnableLike(MatchMode.Anywhere);
                    ICriteria iCriteria = session.CreateCriteria(typeof(T)).Add(example);
                    const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
                    foreach (var item in t.GetProperties(bindingFlags))
                    {
                        if (
                            System.String.Compare(item.PropertyType.Assembly.GetName().Name, "Legal.DataModel",
                                System.StringComparison.Ordinal) != 0) continue;
                        object obj = pModel.GetType().GetProperty(item.Name).GetValue(pModel, null);
                        if (obj == null) continue;
                        Example example1 = Example.Create(obj).IgnoreCase().ExcludeZeroes().EnableLike(MatchMode.Anywhere);
                        iCriteria.CreateCriteria(item.Name).Add(example1);
                        object id = obj.GetType().GetProperty("ID").GetValue(obj, null);
                        if (id != null && System.String.Compare(id.ToString(), "0", System.StringComparison.Ordinal) != 0)
                        {
                            iCriteria.Add(Expression.Eq(item.Name + ".ID", id));
                        }
                    }
                    return iCriteria.SetFirstResult(pageNo * pageSize).SetMaxResults(pageSize).List<T>();
                }
            }
            catch (Exception ex)
            {
                List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                {
                     new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => pModel),
                        Value = JObject.FromObject(pModel).ToString()
                    },
                    new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => pageNo),
                        Value = pageNo.ToString()
                    },
                                        new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => pageSize),
                        Value = pageSize.ToString()
                    },
                };
                throw new MyExceptionHandler(ex.ToString(), ex, JArray.FromObject(lst).ToString());
            }
        }

        /// <summary>
        /// انتخاب رکورد براساس مقادیر فیلدهای پر شده مدل
        /// </summary>
        /// <param name="pModel">مدلی که صفر،یک یا چند فیلد آن پر می باشد</param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<T> FindObjectWithPageSize(T pModel, int pageNo, short pageSize)
        {
            pageNo -= 1;
            try
            {
                Type t = typeof(T);
                using (ISession session = NHibernateConfiguration.OpenSession())
                {
                    Example example = Example.Create(pModel).IgnoreCase().EnableLike(MatchMode.Anywhere);
                    ICriteria iCriteria = session.CreateCriteria(typeof(T)).Add(example);
                    const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
                    foreach (var item in t.GetProperties(bindingFlags))
                    {
                        if (System.String.Compare(item.PropertyType.Assembly.GetName().Name, "LegalMelli.DataModel", System.StringComparison.Ordinal) == 0)
                        {
                            object obj = pModel.GetType().GetProperty(item.Name).GetValue(pModel, null);
                            if (obj != null)
                            {
                                Example example1 = Example.Create(obj).IgnoreCase().EnableLike(MatchMode.Anywhere);
                                iCriteria.CreateCriteria(item.Name).Add(example1);
                                object id = obj.GetType().GetProperty("ID").GetValue(obj, null);
                                if (id != null && !id.Equals(0))
                                {
                                    iCriteria.Add(Expression.Eq(item.Name + ".ID", id));
                                }
                            }
                        }
                    }
                    return iCriteria.SetFirstResult(pageNo * pageSize).SetMaxResults(pageSize).List<T>();
                }
            }
            catch (Exception ex)
            {
                List<ActionInputViewModel> lst = new List<ActionInputViewModel>()
                {
                     new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => pModel),
                        Value = JObject.FromObject(pModel).ToString()
                    },
                    new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => pageNo),
                        Value = pageNo.ToString()
                    },
                                        new ActionInputViewModel()
                    {
                        Name = HelperFunctionInBL.GetVariableName(() => pageSize),
                        Value = pageSize.ToString()
                    },
                };
                throw new MyExceptionHandler(ex.ToString(), ex, JArray.FromObject(lst).ToString());
            }
        }

        /// <summary>
        /// _evict
        /// رو برای این استفاده میکرد که میخواست از کش دربیاره اطلاعات رو
        /// 
        /// کللا اینایی که با کوئری سلکت میکنن و از نوع جدیدی هستند مثل ویومدل هامون، باید براشون فایل مپ هم درست کنیم
        /// و نحوه ی استفادشونم اینه که جنریک ریپازیتوری رو با اونا نیو کنیمو ادامه کار
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public IList<T> SelectWithSqlQuery(string queryString)
        {
            try
            {
                using (ISession session = NHibernateConfiguration.OpenSession())
                {
                    IQuery query = session.CreateSQLQuery(queryString).AddEntity(typeof(T));
                    IList<T> list = query.List<T>();
                    //if (_evict)
                    //{
                    //    foreach (T s in list)
                    //    {
                    //        session.Evict(s);
                    //    }
                    //}
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, queryString);
            }
        }


        public IList<T> SelectWithSqlQueryWhitOutCommitTransaction(string queryString, ISession session)
        {
            try
            {
                IQuery query = session.CreateSQLQuery(queryString).AddEntity(typeof(T));
                IList<T> list = query.List<T>();
                //if (_evict)
                //{
                //    foreach (T s in list)
                //    {
                //        session.Evict(s);
                //    }
                //}
                return list;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, queryString);
            }
        }


        public int UpdatetWithSqlQuery(string queryString)
        {
            try
            {
                using (ISession session = NHibernateConfiguration.OpenSession())
                {
                    IQuery query = session.CreateSQLQuery(queryString).AddEntity(typeof(T));
                    int rowsUpdate = query.ExecuteUpdate();
                    //if (_evict)
                    //{
                    //    foreach (T s in list)
                    //    {
                    //        session.Evict(s);
                    //    }
                    //}
                    return rowsUpdate;
                }
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, queryString);
            }
        }

        public int UpdatetWithSqlQueryWhitOutCommitTransaction(string queryString, ISession session)
        {
            try
            {
                IQuery query = session.CreateSQLQuery(queryString).AddEntity(typeof(T));
                int rowsUpdate = query.ExecuteUpdate();
                return rowsUpdate;
            }
            catch (Exception ex)
            {
                throw new MyExceptionHandler(ex.ToString(), ex, queryString);
            }
        }

    }
}

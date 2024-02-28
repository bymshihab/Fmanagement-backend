using FarmProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace FarmProject.DetailData
{
    public class PurchaseDetailData
    {
        public void InsertPurchaseDetails(List<PurchaseDetail> purchaseDetails, SqlConnection con, SqlTransaction transaction, int purchaseId)
        {
            try
            {
                foreach (var purchaseDetail in purchaseDetails)
                {
                    string systemCode = string.Empty;
                    using (SqlCommand makeSystemCodeCmd = new SqlCommand("spMakeSystemCode", con, transaction))
                    {
                        makeSystemCodeCmd.CommandType = CommandType.StoredProcedure;
                        makeSystemCodeCmd.Parameters.AddWithValue("@TableName", "PurchaseDetails");
                        makeSystemCodeCmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                        makeSystemCodeCmd.Parameters.AddWithValue("@AddNumber", 1);
                        systemCode = makeSystemCodeCmd.ExecuteScalar()?.ToString();
                    }
                    string BatchCode = systemCode.Split('%')[1];
                    SqlCommand detailCmd = new SqlCommand("INSERT INTO PurchaseDetails (PurchaseId,BatchCode, ExpireDate, CategoryId, ProductId, UomId, UnitPrice, Qty, GrossTotal, DiscountAmt, DiscountPct, VatAmt, VatPct, NetTotal, AddedBy, AddedDate, AddedPc, UpdatedBy, UpdatedDate, UpDatedPc) VALUES (@PurchaseId, @BatchCode, @ExpireDate, @CategoryId, @ProductId, @UomId, @UnitPrice, @Qty, @GrossTotal, @DiscountAmt, @DiscountPct, @VatAmt, @VatPct, @NetTotal, @AddedBy, CONVERT(DATETIME, @AddedDate), @AddedPc, @UpdatedBy, CONVERT(DATETIME, @UpdatedDate), @UpDatedPc);", con, transaction);

                    detailCmd.Parameters.AddWithValue("@PurchaseId", purchaseId);
                    detailCmd.Parameters.AddWithValue("@BatchCode", BatchCode);
                    detailCmd.Parameters.AddWithValue("@ExpireDate", purchaseDetail.ExpireDate ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@CategoryId", purchaseDetail.CategoryId);
                    detailCmd.Parameters.AddWithValue("@ProductId", purchaseDetail.ProductId);
                    detailCmd.Parameters.AddWithValue("@UomId", purchaseDetail.UomId);
                    detailCmd.Parameters.AddWithValue("@UnitPrice", purchaseDetail.UnitPrice);
                    detailCmd.Parameters.AddWithValue("@Qty", purchaseDetail.Qty);
                    detailCmd.Parameters.AddWithValue("@GrossTotal", purchaseDetail.GrossTotal);
                    detailCmd.Parameters.AddWithValue("@DiscountAmt", purchaseDetail.DiscountAmt ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@DiscountPct", purchaseDetail.DiscountPct ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@VatAmt", purchaseDetail.VatAmt ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@VatPct", purchaseDetail.VatPct ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@NetTotal", purchaseDetail.NetTotal);
                    detailCmd.Parameters.AddWithValue("@AddedBy", purchaseDetail.AddedBy ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@AddedDate", purchaseDetail.AddedDate ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@AddedPc", purchaseDetail.AddedPc ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@UpdatedBy", purchaseDetail.UpdatedBy ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@UpdatedDate", purchaseDetail.UpdatedDate ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@UpDatedPc", purchaseDetail.UpDatedPc ?? (object)DBNull.Value);

                    detailCmd.ExecuteNonQuery();
                }
            }
            finally
            {
            }
        }

        public void DeleteAllDetails(SqlConnection con, int purchaseId)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM PurchaseDetails WHERE purchaseId = @purchaseId;", con);
            cmd.Parameters.AddWithValue("@purchaseId", purchaseId);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void UpdatePurchaseDetails(List<PurchaseDetail> purchaseDetails, SqlConnection con, int purchaseId)
        {
            foreach (var purchaseDetailTerms in purchaseDetails)
            {
                SqlCommand PDTermsTermsCMD = new SqlCommand("UPDATE PurchaseDetails SET ExpireDate = @ExpireDate, CategoryId = @CategoryId, ProductId = @ProductId, UomId = @UomId, UnitPrice = @UnitPrice,UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate, UpdatedPc = @UpdatedPc WHERE PurchaseId = @PurchaseId", con);


                PDTermsTermsCMD.Parameters.AddWithValue("@purchaseId", purchaseId);
                PDTermsTermsCMD.Parameters.AddWithValue("@ExpireDate", purchaseDetailTerms.ExpireDate ?? (object)DBNull.Value);
                PDTermsTermsCMD.Parameters.AddWithValue("@CategoryId", purchaseDetailTerms.CategoryId);
                PDTermsTermsCMD.Parameters.AddWithValue("@ProductId", purchaseDetailTerms.ProductId);
                PDTermsTermsCMD.Parameters.AddWithValue("@UomId", purchaseDetailTerms.UomId);
                PDTermsTermsCMD.Parameters.AddWithValue("@UnitPrice", purchaseDetailTerms.UnitPrice);
                PDTermsTermsCMD.Parameters.AddWithValue("@Qty", purchaseDetailTerms.Qty);
                PDTermsTermsCMD.Parameters.AddWithValue("@GrossTotal", purchaseDetailTerms.GrossTotal);
                PDTermsTermsCMD.Parameters.AddWithValue("@DiscountAmt", purchaseDetailTerms.DiscountAmt);
                PDTermsTermsCMD.Parameters.AddWithValue("@DiscountPct", purchaseDetailTerms.DiscountPct);
                PDTermsTermsCMD.Parameters.AddWithValue("@VatAmt", purchaseDetailTerms.VatAmt);
                PDTermsTermsCMD.Parameters.AddWithValue("@VatPct", purchaseDetailTerms.VatPct);
                PDTermsTermsCMD.Parameters.AddWithValue("@NetTotal", purchaseDetailTerms.NetTotal);
                PDTermsTermsCMD.Parameters.AddWithValue("@UpdatedBy", purchaseDetailTerms.UpdatedBy ?? (object)DBNull.Value);
                PDTermsTermsCMD.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                PDTermsTermsCMD.Parameters.AddWithValue("@UpDatedPc", purchaseDetailTerms.UpDatedPc ?? (object)DBNull.Value);
                con.Open();
                PDTermsTermsCMD.ExecuteNonQuery();
                con.Close();
            }
        }
        public void GetAllDetails(SqlConnection con, int PurchaseId, List<GetPurchaseDetailsById> Master)
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                using (SqlCommand cmd1 = new SqlCommand("GetPurchaseDetailsById", con))
                {
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@PurchaseId", PurchaseId);

                    using (SqlDataReader reader1 = cmd1.ExecuteReader())
                    {
                        while (reader1.Read())
                        {
                            GetPurchaseDetailsById PDById = new GetPurchaseDetailsById();
                            PDById.PurchaseId = Convert.ToInt32(reader1["PurchaseId"].ToString());
                            PDById.BatchCode = reader1["BatchCode"].ToString();

                            //PDById.ExpireDate = (DateTime)reader1["ExpireDate"] != DBNull.Value ? (DateTime)reader["PurchaseDate"] : default(DateTime);
                            //PurchaseDate = reader["PurchaseDate"] != DBNull.Value ? (DateTime)reader["PurchaseDate"] : default(DateTime),
                            PDById.CategoryId = Convert.ToInt32(reader1["CategoryId"].ToString());
                            PDById.CategoryName = reader1["CategoryName"].ToString();
                            PDById.ProductId = Convert.ToInt32(reader1["ProductId"].ToString());
                            PDById.ProductName = reader1["ProductName"].ToString();
                            PDById.UomId = Convert.ToInt32(reader1["UomId"].ToString());
                            PDById.UomName = reader1["UomName"].ToString();
                            PDById.UnitPrice = (decimal)reader1["UnitPrice"];
                            PDById.Qty = (decimal)reader1["Qty"];
                            PDById.GrossTotal = (decimal)reader1["GrossTotal"];
                            PDById.DiscountAmt = (decimal)reader1["DiscountAmt"];
                            PDById.DiscountPct = (decimal)reader1["DiscountPct"];
                            PDById.VatAmt = (decimal)reader1["VatAmt"];
                            PDById.VatPct = (decimal)reader1["VatPct"];
                            PDById.NetTotal = (decimal)reader1["NetTotal"];
                            Master.Add(PDById);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

    }
}

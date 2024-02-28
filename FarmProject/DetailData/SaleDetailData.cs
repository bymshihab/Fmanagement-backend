using FarmProject.Models;
using System.Data;
using System.Data.SqlClient;

namespace FarmProject.DetailData
{
    public class SaleDetailData
    {
        public void InsertSaleDetails(List<SaleDetail> saleDetails, SqlConnection con, SqlTransaction transaction, int SaleId)
        {
            try
            {
                foreach (var saleDetail in saleDetails)
                {
                    SqlCommand detailCmd = new SqlCommand("INSERT INTO SaleDetails (SaleId, CategoryId, ProductId, UomId, UnitPrice, Qty, GrossTotal, DiscountAmt, DiscountPct, VatAmt, VatPct, NetTotal, AddedBy, AddedDate, AddedPc, UpdatedBy, UpdatedDate, UpDatedPc) VALUES (@SaleId, @CategoryId, @ProductId, @UomId, @UnitPrice, @Qty, @GrossTotal, @DiscountAmt, @DiscountPct, @VatAmt, @VatPct, @NetTotal, @AddedBy, @AddedDate, @AddedPc, @UpdatedBy, @UpdatedDate, @UpDatedPc);", con, transaction);

                    detailCmd.Parameters.AddWithValue("@SaleId", SaleId);
                    detailCmd.Parameters.AddWithValue("@CategoryId", saleDetail.CategoryId);
                    detailCmd.Parameters.AddWithValue("@ProductId", saleDetail.ProductId);
                    detailCmd.Parameters.AddWithValue("@UomId", saleDetail.UomId);
                    detailCmd.Parameters.AddWithValue("@UnitPrice", saleDetail.UnitPrice);
                    detailCmd.Parameters.AddWithValue("@Qty", saleDetail.Qty);
                    detailCmd.Parameters.AddWithValue("@GrossTotal", saleDetail.GrossTotal ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@DiscountAmt", saleDetail.DiscountAmt ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@DiscountPct", saleDetail.DiscountPct ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@VatAmt", saleDetail.VatAmt ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@VatPct", saleDetail.VatPct ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@NetTotal", saleDetail.NetTotal );
                    detailCmd.Parameters.AddWithValue("@AddedBy", saleDetail.AddedBy ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@AddedDate", saleDetail.AddedDate ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@AddedPc", saleDetail.AddedPc ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@UpdatedBy", saleDetail.UpdatedBy ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@UpdatedDate", saleDetail.UpdatedDate ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@UpDatedPc", saleDetail.UpDatedPc ?? (object)DBNull.Value);

                    detailCmd.ExecuteNonQuery();
                }
            }
            finally
            {
            }
        }
        public void UpdateSaleDetails(List<SaleDetail> saleDetails, SqlConnection con, int SaleId)
        {
            foreach (var saleDetailTerms in saleDetails)
            {
                SqlCommand SaleTermsCMD = new SqlCommand("UPDATE SaleDetails SET CategoryId = @CategoryId, ProductId = @ProductId, UomId = @UomId, UnitPrice = @UnitPrice,UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate, UpdatedPc = @UpdatedPc WHERE SaleId = @SaleId", con);


                SaleTermsCMD.Parameters.AddWithValue("@SaleId", SaleId);
                SaleTermsCMD.Parameters.AddWithValue("@CategoryId", saleDetailTerms.CategoryId);
                SaleTermsCMD.Parameters.AddWithValue("@ProductId", saleDetailTerms.ProductId);
                SaleTermsCMD.Parameters.AddWithValue("@UomId", saleDetailTerms.UomId);
                SaleTermsCMD.Parameters.AddWithValue("@UnitPrice", saleDetailTerms.UnitPrice);
                SaleTermsCMD.Parameters.AddWithValue("@Qty", saleDetailTerms.Qty);
                SaleTermsCMD.Parameters.AddWithValue("@GrossTotal", saleDetailTerms.GrossTotal ?? (object)DBNull.Value);
                SaleTermsCMD.Parameters.AddWithValue("@DiscountAmt", saleDetailTerms.DiscountAmt ?? (object)DBNull.Value);
                SaleTermsCMD.Parameters.AddWithValue("@DiscountPct", saleDetailTerms.DiscountPct ?? (object)DBNull.Value);
                SaleTermsCMD.Parameters.AddWithValue("@VatAmt", saleDetailTerms.VatAmt ?? (object)DBNull.Value);
                SaleTermsCMD.Parameters.AddWithValue("@VatPct", saleDetailTerms.VatPct ?? (object)DBNull.Value);
                SaleTermsCMD.Parameters.AddWithValue("@NetTotal", saleDetailTerms.NetTotal);
                SaleTermsCMD.Parameters.AddWithValue("@UpdatedBy", saleDetailTerms.UpdatedBy ?? (object)DBNull.Value);
                SaleTermsCMD.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                SaleTermsCMD.Parameters.AddWithValue("@UpDatedPc", saleDetailTerms.UpDatedPc ?? (object)DBNull.Value);
                con.Open();
                SaleTermsCMD.ExecuteNonQuery();
                con.Close();
            }
        }
        public void DeleteAllDetails(SqlConnection con, int SaleId)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM SaleDetails WHERE SaleId = @SaleId;", con);
            cmd.Parameters.AddWithValue("@SaleId", SaleId);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void GetAllDetails(SqlConnection con, int SaleId, List<SaleDetailsById> Master)
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                using (SqlCommand cmd1 = new SqlCommand("GetSaleDetailsById", con))
                {
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@SaleId", SaleId);

                    using (SqlDataReader reader1 = cmd1.ExecuteReader())
                    {
                        while (reader1.Read())
                        {
                            SaleDetailsById PDById = new SaleDetailsById();
                            PDById.SaleId = Convert.ToInt32(reader1["SaleId"].ToString());
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

using FarmProject.Models;
using System.Data;
using System.Data.SqlClient;

namespace FarmProject.DetailData
{
    public class FeedingDetailData
    {
        public void InsertFeedingDetails(List<FeedingDetail> feedingDetails, SqlConnection con, SqlTransaction transaction, int FeedId)
        {
            try
            {
                foreach (var feedingDetail in feedingDetails)
                {
                    SqlCommand detailCmd = new SqlCommand("INSERT INTO FeedingDetails (FeedId, ProductId, Price, Qty, TotalPrice, UomId, AddedBy, AddedDate, AddedPc, UpdatedBy, UpdatedDate, UpDatedPc) VALUES (@FeedId, @ProductId, @Price, @Qty, @TotalPrice, @UomId, @AddedBy, CONVERT(DATETIME, @AddedDate), @AddedPc, @UpdatedBy, CONVERT(DATETIME, @UpdatedDate), @UpDatedPc);", con, transaction);

                    detailCmd.Parameters.AddWithValue("@FeedId", FeedId);
                    detailCmd.Parameters.AddWithValue("@ProductId", feedingDetail.ProductId);
                    detailCmd.Parameters.AddWithValue("@Price", feedingDetail.Price);
                    detailCmd.Parameters.AddWithValue("@Qty", feedingDetail.Qty);
                    detailCmd.Parameters.AddWithValue("@TotalPrice", feedingDetail.TotalPrice);
                    detailCmd.Parameters.AddWithValue("@UomId", feedingDetail.UomId);
                    detailCmd.Parameters.AddWithValue("@AddedBy", feedingDetail.AddedBy ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@AddedDate", DateTime.Now);
                    detailCmd.Parameters.AddWithValue("@AddedPc", feedingDetail.AddedPc ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@UpdatedBy", feedingDetail.UpdatedBy ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@UpdatedDate", feedingDetail.UpdatedDate ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@UpDatedPc", feedingDetail.UpDatedPc ?? (object)DBNull.Value);

                    detailCmd.ExecuteNonQuery();
                }
            }
            finally
            {
            }
        }
        public void UpdateFeedingDetails(List<FeedingDetail> feedingDetails, SqlConnection con, int FeedId)
        {
            foreach (var feedingDetailTerms in feedingDetails)
            {
                SqlCommand PDTermsTermsCMD = new SqlCommand("UPDATE FeedingDetails SET  ProductId = @ProductId, Price = @Price, Qty = @Qty, TotalPrice=@TotalPrice, UomId = @UomId, UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate, UpdatedPc = @UpdatedPc WHERE FeedId = @FeedId", con);


                PDTermsTermsCMD.Parameters.AddWithValue("@FeedId", FeedId);
                PDTermsTermsCMD.Parameters.AddWithValue("@ProductId", feedingDetailTerms.ProductId);
                PDTermsTermsCMD.Parameters.AddWithValue("@Price", feedingDetailTerms.Price);
                PDTermsTermsCMD.Parameters.AddWithValue("@Qty", feedingDetailTerms.Qty);
                PDTermsTermsCMD.Parameters.AddWithValue("@TotalPrice", feedingDetailTerms.TotalPrice);
                PDTermsTermsCMD.Parameters.AddWithValue("@UomId", feedingDetailTerms.UomId);
                PDTermsTermsCMD.Parameters.AddWithValue("@UpdatedBy", feedingDetailTerms.UpdatedBy ?? (object)DBNull.Value);
                PDTermsTermsCMD.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                PDTermsTermsCMD.Parameters.AddWithValue("@UpDatedPc", feedingDetailTerms.UpDatedPc ?? (object)DBNull.Value);
                con.Open();
                PDTermsTermsCMD.ExecuteNonQuery();
                con.Close();
            }
        }
        public void DeleteAllDetails(SqlConnection con, int FeedId)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM FeedingDetails WHERE FeedId = @FeedId;", con);
            cmd.Parameters.AddWithValue("@FeedId", FeedId);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }


        public void GetAllDetails(SqlConnection con, int FeedId, List<FeedingDetailsGetById> Master)
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                using (SqlCommand cmd1 = new SqlCommand("FeedingDetailsGetById", con))
                {
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@FeedId", FeedId);

                    using (SqlDataReader reader1 = cmd1.ExecuteReader())
                    {
                        while (reader1.Read())
                        {
                            FeedingDetailsGetById PDById = new FeedingDetailsGetById();
                            PDById.FeedId = Convert.ToInt32(reader1["FeedId"].ToString());
                            PDById.ProductId = Convert.ToInt32(reader1["ProductId"].ToString());
                            PDById.ProductName = reader1["ProductName"].ToString();
                            PDById.Price = (decimal)reader1["Price"];
                            PDById.Qty = (decimal)reader1["Qty"];
                            PDById.TotalPrice = (decimal)reader1["TotalPrice"];
                            PDById.UomId = Convert.ToInt32(reader1["UomId"].ToString());
                            PDById.UomName = reader1["UomName"].ToString();


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

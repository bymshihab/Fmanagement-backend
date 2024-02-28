using FarmProject.Models;
using System.Data;
using System.Data.SqlClient;

namespace FarmProject.DetailData
{
    public class GrainDetailData
    {
        public void InsertGrainDetails(List<GrainFeedChart> grainFeedCharts, SqlConnection con, SqlTransaction transaction, int GrainMasterId)
        {
            try
            {
                foreach (var grainFeedChart in grainFeedCharts)
                {
                    SqlCommand detailCmd = new SqlCommand("INSERT INTO grainFeedCharts (GrainMasterId, ProductId, Price, Qty, UomId, TotalPrice, AddedBy, AddedDate, AddedPc, UpdatedBy, UpdatedDate, UpDatedPc) VALUES (@GrainMasterId, @ProductId, @Price, @Qty, @UomId, @TotalPrice, @AddedBy, CONVERT(DATETIME, @AddedDate), @AddedPc, @UpdatedBy, CONVERT(DATETIME, @UpdatedDate), @UpDatedPc);", con, transaction);

                    detailCmd.Parameters.AddWithValue("@GrainMasterId", GrainMasterId);
                    detailCmd.Parameters.AddWithValue("@ProductId", grainFeedChart.ProductId);
                    detailCmd.Parameters.AddWithValue("@Price", grainFeedChart.Price);
                    detailCmd.Parameters.AddWithValue("@Qty", grainFeedChart.Qty);
                    detailCmd.Parameters.AddWithValue("@UomId", grainFeedChart.UomId);
                    detailCmd.Parameters.AddWithValue("@TotalPrice", grainFeedChart.TotalPrice);
                    detailCmd.Parameters.AddWithValue("@AddedBy", grainFeedChart.AddedBy ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@AddedDate", DateTime.Now);
                    detailCmd.Parameters.AddWithValue("@AddedPc", grainFeedChart.AddedPc ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@UpdatedBy", grainFeedChart.UpdatedBy ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@UpdatedDate", grainFeedChart.UpdatedDate ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@UpDatedPc", grainFeedChart.UpDatedPc ?? (object)DBNull.Value);

                    detailCmd.ExecuteNonQuery();
                }
            }
            finally
            {
            }
        }
        public void UpdateGrainDetails(List<GrainFeedChart> grainFeedCharts, SqlConnection con, int GrainMasterId)
        {
            foreach (var grainFeedChart in grainFeedCharts)
            {
                SqlCommand PDTermsTermsCMD = new SqlCommand("UPDATE grainFeedCharts SET  ProductId = @ProductId, Price = @Price, Qty = @Qty, TotalPrice = @TotalPrice, UomId = @UomId, UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate, UpdatedPc = @UpdatedPc WHERE GrainMasterId = @GrainMasterId", con);


                PDTermsTermsCMD.Parameters.AddWithValue("@GrainMasterId", GrainMasterId);
                PDTermsTermsCMD.Parameters.AddWithValue("@ProductId", grainFeedChart.ProductId);
                PDTermsTermsCMD.Parameters.AddWithValue("@Price", grainFeedChart.Price);
                PDTermsTermsCMD.Parameters.AddWithValue("@Qty", grainFeedChart.Qty);
                PDTermsTermsCMD.Parameters.AddWithValue("@UomId", grainFeedChart.UomId);
                PDTermsTermsCMD.Parameters.AddWithValue("@TotalPrice", grainFeedChart.TotalPrice);
                PDTermsTermsCMD.Parameters.AddWithValue("@UpdatedBy", grainFeedChart.UpdatedBy ?? (object)DBNull.Value);
                PDTermsTermsCMD.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                PDTermsTermsCMD.Parameters.AddWithValue("@UpDatedPc", grainFeedChart.UpDatedPc ?? (object)DBNull.Value);
                con.Open();
                PDTermsTermsCMD.ExecuteNonQuery();
                con.Close();
            }
        }

        public void DeleteAllDetails(SqlConnection con, int GrainMasterId)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM grainFeedCharts WHERE GrainMasterId = @GrainMasterId;", con);
            cmd.Parameters.AddWithValue("@GrainMasterId", GrainMasterId);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void GetAllDetails(SqlConnection con, int GrainMasterId, List<GrainDetailsById> Master)
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                using (SqlCommand cmd1 = new SqlCommand("GrainDetailsById", con))
                {
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@GrainMasterId", GrainMasterId);

                    using (SqlDataReader reader1 = cmd1.ExecuteReader())
                    {
                        while (reader1.Read())
                        {
                            GrainDetailsById PDById = new GrainDetailsById();
                            PDById.GrainMasterId = Convert.ToInt32(reader1["GrainMasterId"].ToString());
                            PDById.ProductId = Convert.ToInt32(reader1["ProductId"].ToString());
                            PDById.ProductName = reader1["ProductName"].ToString();
                            PDById.Price = (decimal)reader1["Price"];
                            PDById.Qty = (decimal)reader1["Qty"];
                            PDById.UomId = Convert.ToInt32(reader1["UomId"].ToString());
                            PDById.UomName = reader1["UomName"].ToString();
                            PDById.TotalPrice = (decimal)reader1["TotalPrice"];

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

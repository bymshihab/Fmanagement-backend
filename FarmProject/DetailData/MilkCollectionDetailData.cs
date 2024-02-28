using FarmProject.Models;
using System.Data;
using System.Data.SqlClient;

namespace FarmProject.DetailData
{
    public class MilkCollectionDetailData
    {
        public void InsertMilkCollectionDetails(List<MilkCollectionDetail> milkCollectionDetails, SqlConnection con, SqlTransaction transaction, int MilkCollectionId)
        {
            try
            {
                foreach (var milkCollectionDetail in milkCollectionDetails)
                {
                    SqlCommand detailCmd = new SqlCommand("INSERT INTO MilkCollectionDetails (MilkCollectionId, AnimalId, MilkId, Qty, UomId, AddedBy, AddedDate, AddedPc, UpdatedBy, UpdatedDate, UpDatedPc) VALUES (@MilkCollectionId, @AnimalId, @MilkId, @Qty, @UomId, @AddedBy, CONVERT(DATETIME, @AddedDate), @AddedPc, @UpdatedBy, CONVERT(DATETIME, @UpdatedDate), @UpDatedPc);", con, transaction);

                    detailCmd.Parameters.AddWithValue("@MilkCollectionId", MilkCollectionId);
                    detailCmd.Parameters.AddWithValue("@AnimalId", milkCollectionDetail.AnimalId);
                    detailCmd.Parameters.AddWithValue("@MilkId", milkCollectionDetail.MilkId);
                    detailCmd.Parameters.AddWithValue("@Qty", milkCollectionDetail.Qty);
                    detailCmd.Parameters.AddWithValue("@UomId", milkCollectionDetail.UomId);
                    detailCmd.Parameters.AddWithValue("@AddedBy", milkCollectionDetail.AddedBy ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@AddedDate", DateTime.Now);
                    detailCmd.Parameters.AddWithValue("@AddedPc", milkCollectionDetail.AddedPc ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@UpdatedBy", milkCollectionDetail.UpdatedBy ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@UpdatedDate", milkCollectionDetail.UpdatedDate ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@UpDatedPc", milkCollectionDetail.UpDatedPc ?? (object)DBNull.Value);

                    detailCmd.ExecuteNonQuery();
                }
            }
            finally
            {
            }
        }
        public void UpdateMilkCollection(List<MilkCollectionDetail> milkCollectionDetails, SqlConnection con, int MilkCollectionId)
        {
            foreach (var milkCollectionDetail in milkCollectionDetails)
            {
                SqlCommand PDTermsTermsCMD = new SqlCommand("UPDATE MilkCollectionDetails SET  AnimalId = @AnimalId, Qty = @Qty, UomId = @UomId, UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate, UpdatedPc = @UpdatedPc WHERE MilkCollectionId = @MilkCollectionId", con);


                PDTermsTermsCMD.Parameters.AddWithValue("@MilkCollectionId", MilkCollectionId);
                PDTermsTermsCMD.Parameters.AddWithValue("@AnimalId", milkCollectionDetail.AnimalId);
                PDTermsTermsCMD.Parameters.AddWithValue("@MilkId", milkCollectionDetail.MilkId);
                PDTermsTermsCMD.Parameters.AddWithValue("@Qty", milkCollectionDetail.Qty);
                PDTermsTermsCMD.Parameters.AddWithValue("@UomId", milkCollectionDetail.UomId);
                PDTermsTermsCMD.Parameters.AddWithValue("@UpdatedBy", milkCollectionDetail.UpdatedBy ?? (object)DBNull.Value);
                PDTermsTermsCMD.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                PDTermsTermsCMD.Parameters.AddWithValue("@UpDatedPc", milkCollectionDetail.UpDatedPc ?? (object)DBNull.Value);
                con.Open();
                PDTermsTermsCMD.ExecuteNonQuery();
                con.Close();
            }
        }
        public void DeleteAllDetails(SqlConnection con, int MilkCollectionId)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM MilkCollectionDetails WHERE MilkCollectionId = @MilkCollectionId;", con);
            cmd.Parameters.AddWithValue("@MilkCollectionId", MilkCollectionId);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }


        public void GetAllDetails(SqlConnection con, int MilkCollectionId, List<MilkCollectionDetailById> Master)
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                using (SqlCommand cmd1 = new SqlCommand("MilkCollectionDetailsById", con))
                {
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@MilkCollectionId", MilkCollectionId);

                    using (SqlDataReader reader1 = cmd1.ExecuteReader())
                    {
                        while (reader1.Read())
                        {
                            MilkCollectionDetailById PDById = new MilkCollectionDetailById();
                            PDById.MilkCollectionId = Convert.ToInt32(reader1["MilkCollectionId"].ToString());
                            PDById.AnimalId = Convert.ToInt32(reader1["AnimalId"].ToString());
                            PDById.AnimalName = reader1["AnimalName"].ToString();
                            PDById.MilkId = Convert.ToInt32(reader1["MilkId"].ToString());
                            PDById.Milktype = reader1["Milktype"].ToString();
                            PDById.Qty = (decimal)reader1["Qty"];
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

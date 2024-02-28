using FarmProject.Models;
using System.Data;
using System.Data.SqlClient;

namespace FarmProject.DetailData
{
    public class MedicalExhibitionData
    {
        public void InsertMedicineDetails(List<Medicine> medicines, SqlConnection con, SqlTransaction transaction, int MedicalId)
        {
            try
            {
                foreach (var MDetail in medicines)
                {
                    SqlCommand detailCmd = new SqlCommand("INSERT INTO Medicines (MedicalId, ProductId, time, day, Qty, UomId, AddedBy, AddedDate, AddedPc, UpdatedBy, UpdatedDate, UpDatedPc) VALUES (@MedicalId, @ProductId, @time, @day, @Qty, @UomId, @AddedBy, CONVERT(DATETIME, @AddedDate), @AddedPc, @UpdatedBy, CONVERT(DATETIME, @UpdatedDate), @UpDatedPc);", con, transaction);

                    detailCmd.Parameters.AddWithValue("@MedicalId", MedicalId);
                    detailCmd.Parameters.AddWithValue("@ProductId", MDetail.ProductId);
                    detailCmd.Parameters.AddWithValue("@day", MDetail.day);
                    detailCmd.Parameters.AddWithValue("@time", MDetail.time);
                    detailCmd.Parameters.AddWithValue("@Qty", MDetail.Qty);
                    detailCmd.Parameters.AddWithValue("@UomId", MDetail.UomId);
                    detailCmd.Parameters.AddWithValue("@AddedBy", MDetail.AddedBy ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@AddedDate", DateTime.Now);
                    detailCmd.Parameters.AddWithValue("@AddedPc", MDetail.AddedPc ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@UpdatedBy", MDetail.UpdatedBy ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@UpdatedDate", MDetail.UpdatedDate ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@UpDatedPc", MDetail.UpDatedPc ?? (object)DBNull.Value);

                    detailCmd.ExecuteNonQuery();
                }
            }
            finally
            {
            }
        }
        public void InsertQuarantaineDetails(List<Quarantaine> quarantaines, SqlConnection con, SqlTransaction transaction, int MedicalId)
        {
            try
            {
                foreach (var QDetail in quarantaines)
                {
                    SqlCommand detailCmd = new SqlCommand("INSERT INTO Quarantaines (MedicalId, StartDate, EndDate, ShedId, AddedBy, AddedDate, AddedPc, UpdatedBy, UpdatedDate, UpDatedPc) VALUES (@MedicalId, @StartDate, @EndDate, @ShedId, @AddedBy, CONVERT(DATETIME, @AddedDate), @AddedPc, @UpdatedBy, CONVERT(DATETIME, @UpdatedDate), @UpDatedPc);", con, transaction);

                    detailCmd.Parameters.AddWithValue("@MedicalId", MedicalId);
                    detailCmd.Parameters.AddWithValue("@StartDate", QDetail.StartDate);
                    detailCmd.Parameters.AddWithValue("@EndDate", QDetail.EndDate ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@ShedId", QDetail.ShedId);
                    detailCmd.Parameters.AddWithValue("@AddedBy", QDetail.AddedBy ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@AddedDate", DateTime.Now);
                    detailCmd.Parameters.AddWithValue("@AddedPc", QDetail.AddedPc ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@UpdatedBy", QDetail.UpdatedBy ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@UpdatedDate", QDetail.UpdatedDate ?? (object)DBNull.Value);
                    detailCmd.Parameters.AddWithValue("@UpDatedPc", QDetail.UpDatedPc ?? (object)DBNull.Value);

                    detailCmd.ExecuteNonQuery();
                }
            }
            finally
            {
            }
        }
        public void UpdateMedicineDetails(List<Medicine> medicines, SqlConnection con, int MedicalId)
        {
            foreach (var MDetail in medicines)
            {
                SqlCommand PDTermsTermsCMD = new SqlCommand("UPDATE Medicines SET ProductId = @ProductId, day = @day, time = @time, Qty = @Qty, UomId = @UomId, UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate, UpdatedPc = @UpdatedPc WHERE MedicalId = @MedicalId", con);

                PDTermsTermsCMD.Parameters.AddWithValue("@MedicalId", MedicalId);
                PDTermsTermsCMD.Parameters.AddWithValue("@ProductId", MDetail.ProductId ?? (object)DBNull.Value);
                PDTermsTermsCMD.Parameters.AddWithValue("@day", MDetail.day);
                PDTermsTermsCMD.Parameters.AddWithValue("@time", MDetail.time);
                PDTermsTermsCMD.Parameters.AddWithValue("@Qty", MDetail.Qty);
                PDTermsTermsCMD.Parameters.AddWithValue("@UomId", MDetail.UomId);
                PDTermsTermsCMD.Parameters.AddWithValue("@UpdatedBy", MDetail.UpdatedBy ?? (object)DBNull.Value);
                PDTermsTermsCMD.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                PDTermsTermsCMD.Parameters.AddWithValue("@UpDatedPc", MDetail.UpDatedPc ?? (object)DBNull.Value);

                con.Open();
                PDTermsTermsCMD.ExecuteNonQuery();
                con.Close();
            }
        }

        public void UpdateQuarantaineDetails(List<Quarantaine> quarantaines, SqlConnection con, int MedicalId)
        {
            foreach (var QDetail in quarantaines)
            {
                SqlCommand PDTermsTermsCMD = new SqlCommand("UPDATE Quarantaines SET  StartDate = @StartDate, EndDate = @EndDate, ShedId = @ShedId, UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate, UpdatedPc = @UpdatedPc WHERE MedicalId = @MedicalId", con);



                PDTermsTermsCMD.Parameters.AddWithValue("@MedicalId", MedicalId);
                PDTermsTermsCMD.Parameters.AddWithValue("@StartDate", QDetail.StartDate);
                PDTermsTermsCMD.Parameters.AddWithValue("@EndDate", QDetail.EndDate ?? (object)DBNull.Value);
                PDTermsTermsCMD.Parameters.AddWithValue("@ShedId", QDetail.ShedId);
                PDTermsTermsCMD.Parameters.AddWithValue("@UpdatedBy", QDetail.UpdatedBy ?? (object)DBNull.Value);
                PDTermsTermsCMD.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                PDTermsTermsCMD.Parameters.AddWithValue("@UpDatedPc", QDetail.UpDatedPc ?? (object)DBNull.Value);
                con.Open();
                PDTermsTermsCMD.ExecuteNonQuery();
                con.Close();
            }
        }

        public void DeleteAllMedicines(SqlConnection con, int MedicalId)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM Medicines WHERE MedicalId = @MedicalId;", con);
            cmd.Parameters.AddWithValue("@MedicalId", MedicalId);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void DeleteAllQuarantaines(SqlConnection con, int MedicalId)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM Quarantaines WHERE MedicalId = @MedicalId;", con);
            cmd.Parameters.AddWithValue("@MedicalId", MedicalId);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void GetAllMedicineDetails(SqlConnection con, int MedicalId, List<MedicineById> Master)
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                using (SqlCommand cmd1 = new SqlCommand("GetMedicineDetailsByMedicalId", con))
                {
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@MedicalId", MedicalId);

                    using (SqlDataReader reader1 = cmd1.ExecuteReader())
                    {
                        while (reader1.Read())
                        {
                            MedicineById PDById = new MedicineById();
                            PDById.MedicalId = Convert.ToInt32(reader1["MedicalId"].ToString());
                            PDById.ProductId = Convert.ToInt32(reader1["ProductId"].ToString());
                            PDById.ProductName = reader1["ProductName"].ToString();
                            PDById.day = (decimal)reader1["day"];
                            PDById.time = reader1["time"].ToString();
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

        public void GetAllQuarantaineDetails(SqlConnection con, int MedicalId, List<QuarantaineById> Master)
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                using (SqlCommand cmd1 = new SqlCommand("GetQuarantaineDetailsById", con))
                {
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@MedicalId", MedicalId);

                    using (SqlDataReader reader1 = cmd1.ExecuteReader())
                    {
                        while (reader1.Read())
                        {
                            QuarantaineById PDById = new QuarantaineById();
                            PDById.MedicalId = Convert.ToInt32(reader1["MedicalId"].ToString());
                            PDById.StartDate = (DateTime)reader1["StartDate"];
                            PDById.EndDate = (DateTime)reader1["EndDate"];
                            PDById.ShedId = Convert.ToInt32(reader1["ShedId"].ToString());
                            PDById.ShedName = reader1["ShedName"].ToString();
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

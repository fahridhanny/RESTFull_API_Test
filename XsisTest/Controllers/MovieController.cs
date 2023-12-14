using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using XsisTest.Models;

namespace XsisTest.Controllers
{
    public class MovieController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Movie> data = new List<Movie>();
            string constr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = "SELECT * FROM sys.tbl_movie";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            data.Add(new Movie
                            {
                                id = Convert.ToInt32(sdr["id"]),
                                title = sdr["title"].ToString(),
                                description = sdr["description"].ToString(),
                                rating = Convert.ToInt32(sdr["rating"]),
                                image = sdr["image"].ToString(),
                                created_at = Convert.ToDateTime(sdr["created_at"]),
                                updated_at = Convert.ToDateTime(sdr["updated_at"])
                            });
                        }
                    }
                    con.Close();
                }
            }
            return Ok(data);
        }

        [HttpGet]
        public IHttpActionResult GetId(int? id)
        {
            Movie data = new Movie();
            try
            {
                if (id == null)
                {
                    // Log the error or handle the case where the movie object is null.
                    return BadRequest("id tidak boleh kosong");
                }

                string constr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    string query = "SELECT * FROM sys.tbl_movie WHERE id = @id";
                    using (MySqlCommand cmd = new MySqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        cmd.Parameters.AddWithValue("@id", id);

                        using (MySqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if (sdr.Read())
                            {
                                data.id = Convert.ToInt32(sdr["id"]);
                                data.title = sdr["title"].ToString();
                                data.description = sdr["description"].ToString();
                                data.rating = Convert.ToInt32(sdr["rating"]);
                                data.image = sdr["image"].ToString();
                                data.created_at = Convert.ToDateTime(sdr["created_at"]);
                                data.updated_at = Convert.ToDateTime(sdr["updated_at"]);
                            }
                        }

                        con.Close();
                    }
                }
            }
            catch (Exception)
            {
                return BadRequest("Movie tidak ada");
            }
            return Ok(data);
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody] Movie movie)
        {
            Movie data = new Movie();
            try
            {
                if (movie == null)
                {
                    // Log the error or handle the case where the movie object is null.
                    return BadRequest("Request body is empty or not in the expected format.");
                }

                string constr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    string query = "INSERT INTO sys.tbl_movie (title, description, rating, image, created_at, updated_at) VALUES (@title, @description, @rating, @image, @created_at, @updated_at); SELECT LAST_INSERT_ID();";
                    using (MySqlCommand cmd = new MySqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        cmd.Parameters.AddWithValue("@title", movie.title);
                        cmd.Parameters.AddWithValue("@description", movie.description);
                        cmd.Parameters.AddWithValue("@rating", movie.rating);
                        cmd.Parameters.AddWithValue("@image", movie.image);
                        cmd.Parameters.AddWithValue("@created_at", movie.created_at);
                        cmd.Parameters.AddWithValue("@updated_at", movie.updated_at);
                        
                        var Id = cmd.ExecuteScalar();
                        
                        con.Close();

                        data.id = Convert.ToInt32(Id);
                    }
                }

                data.title = movie.title;
                data.description = movie.description;
                data.rating = movie.rating;
                data.image = movie.image;
                data.created_at = movie.created_at;
                data.updated_at = movie.updated_at;
            }
            catch (Exception)
            {
                return BadRequest("Error saat insert data");
            }
            return Ok(data);
        }

        [HttpPatch]
        public IHttpActionResult Edit(int? id, [FromBody] Movie movie)
        {
            Movie data = new Movie();
            try
            {
                if (id == null)
                {
                    // Log the error or handle the case where the movie object is null.
                    return BadRequest("id tidak boleh kosong");
                }

                string constr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    string query = "UPDATE sys.tbl_movie SET title=@title, description=@description, rating=@rating, image=@image, created_at=@created_at, updated_at=@updated_at WHERE id=@id";
                    using (MySqlCommand cmd = new MySqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        cmd.Parameters.AddWithValue("@title", movie.title);
                        cmd.Parameters.AddWithValue("@description", movie.description);
                        cmd.Parameters.AddWithValue("@rating", movie.rating);
                        cmd.Parameters.AddWithValue("@image", movie.image);
                        cmd.Parameters.AddWithValue("@created_at", movie.created_at);
                        cmd.Parameters.AddWithValue("@updated_at", movie.updated_at);
                        cmd.Parameters.AddWithValue("@id", id.Value);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }

                data.id = id.Value;
                data.title = movie.title;
                data.description = movie.description;
                data.rating = movie.rating;
                data.image = movie.image;
                data.created_at = movie.created_at;
                data.updated_at = movie.updated_at;
            }
            catch (Exception)
            {
                return BadRequest("Error saat update data");
            }
            return Ok(data);
        }

        [HttpDelete]
        public IHttpActionResult Delete(int? id)
        {
            var message = "";
            try
            {
                if (id == null)
                {
                    // Log the error or handle the case where the movie object is null.
                    return BadRequest("id tidak boleh kosong");
                }

                string constr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    string query = "DELETE FROM sys.tbl_movie WHERE id=@id";
                    using (MySqlCommand cmd = new MySqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                message = "Delete Movie from Database";
            }
            catch (Exception)
            {
                return BadRequest("Error saat delete data");
            }
            return Ok(message);
        }
    }
}
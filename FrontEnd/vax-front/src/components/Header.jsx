import React, { useEffect, useState } from "react";

import { Link } from "react-router-dom";
import { getAuthToken, removeAuthToken } from "../services/auth";
// import './Navbar.css'; 

const Navbar = () => {
  // useEffect(()=>{
  //   checkTokens()
  // },[])
//   const [boool,setBool]=useState(false);
function checkTokens(){

    removeAuthToken();

}
  return (
    <nav className="navbar">
      <div className="navbar-brand">
        <a href="/">Logo</a>
      </div>
      <ul className="navbar-nav">
      


        <li><Link to="/admin-home">Home</Link></li>
        <li><Link to="/about">about</Link></li>
        <li><Link to="/login" onClick={checkTokens}>login</Link></li> 
      </ul>
    </nav>
  );
};

export default Navbar;
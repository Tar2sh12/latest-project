import React from 'react'
import './patientHeader.css'
import { Link } from "react-router-dom";
import { getAuthToken, removeAuthToken } from "../services/auth";
const VaccineCenterHeader = () => {
    function checkTokens(){
        removeAuthToken();
    }
  return (
    <div>
        <nav className="navbar navbar-expand-lg navbar-dark" style={{ backgroundColor: '#a2daf5' }}>
        <div className="container">
            <a className="navbar-brand" href="#">Vax Scheduler</a>
            <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
            <span className="navbar-toggler-icon"></span>
            </button>
            <div className="collapse navbar-collapse" id="navbarNav">
            <ul className="navbar-nav ml-auto">
                <li className="nav-item"><Link className="nav-link btn btn-outline-light rounded-pill" to="/login" onClick={checkTokens}><a className="nav-link btn btn-outline-light rounded-pill" href="#" style={{ backgroundColor: 'white', color: '#a2daf5' }}>Log in</a></Link></li> 
            </ul>
            </div>
        </div>
        </nav>
    </div>
  )
}

export default VaccineCenterHeader




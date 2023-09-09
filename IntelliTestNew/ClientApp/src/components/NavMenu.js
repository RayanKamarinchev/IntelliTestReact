import React, { useState, useContext } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import { LoginPartial } from './account';
import UserContext from '../auth/user';
import './NavMenu.css';
import {Route} from "react-router";

export const NavMenu = props => {

  const [collapsed, setCollapsed] = useState(true);

  const { userConfig } = useContext(UserContext);

  const toggleNavbar = () => {
    setCollapsed(!collapsed);
  }

  const {PhotoPath} = useAccount();

  return (
      <header>
        <nav className="navbar navbar-expand-lg navbar-light bg-white border-bottom box-shadow">
          <div className="container-fluid">
            <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                    aria-expanded="false" aria-label="Toggle navigation">
              <span className="navbar-toggler-icon"></span>
            </button>
            <div className="navbar-collapse collapse d-lg-inline-flex justify-content-between">
              <ul className="navbar-nav flex-grow-1">
                <li className="nav-item">
                  <Link className="navbar-brand logo" to="/">
                    <span className="logoCol1">Intelli</span><span className="logoCol2">Test</span>
                  </Link>
                </li>
                {(userConfig.signedIn) && (
                    <>
                      <li className="nav-item align-items-center d-flex">
                        <Link className="nav-link text-dark" to="/Tests">Тестове</Link>
                      </li>
                      <li className="nav-item align-items-center d-flex">
                        <Link class="nav-link text-dark" to="/Lessons">Уроци</Link>
                      </li>
                      <li className="nav-item align-items-center d-flex">
                        <Link class="nav-link text-dark" to="/Chat">Чат</Link>
                      </li>
                      <li className="nav-item align-items-center d-flex">
                        <Link className="nav-link text-dark" to="/Classes">Класове</Link>
                      </li>
                    </>
                )}
              </ul>
              {(userConfig.signedIn) && (
                  <>
                    string src = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460__340.png";
                    if (!string.IsNullOrEmpty(user.PhotoPath))
                    {
                      src = "/" + user.PhotoPath;
                    }
                    <li style="display: flex;
                                    align-items: center;">
                      <a style="font-weight: 500;">@(user.FirstName +" "+ user.LastName[0]+".")</a>
                    </li>
                    <li class="nav-item">

                      <a asp-action="ViewProfile" asp-controller="User" class="nav-link text-dark">
                        <img src="@src" alt="" style="width: 40px; height: 40px; margin-left: 10px; border-radius: 100%;">
                      </a>
                    </li>
                    <li class="nav-item">
                      <a type="submit" class="nav-link text-dark" asp-controller="User" asp-action="Logout"
                         asp-route-Id="@UserManager.GetUserId(User)">
                        <i class="fa fa-sign-out"></i>
                        <span>Logout</span>
                      </a>
                    </li>
                  </>
              )}
            </div>
          </div>
        </nav>
      </header>
  );
}

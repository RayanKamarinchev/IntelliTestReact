import { BrowserRouter as Router, Route, Switch} from 'react-router-dom';
import './App.css';
import Navbar from "./Navbar";
import Home from "./Home";
import Footer from "./Footer";
import Blog from "./Blog";

function App() {
    return (
        <Router>
            <div className="App">
                <Navbar />
                <div className="content">
                    <Switch>
                        <Route exact path="/">
                            <Home />
                        </Route>
                        <Route exact path="/blog">
                            <Blog />
                        </Route>
                    </Switch>
                </div>
                <Footer/>
            </div>
        </Router>
    );
}

export default App;

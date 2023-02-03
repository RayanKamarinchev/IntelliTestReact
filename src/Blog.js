import React from 'react';
import Card from "./smallComponents/Card";
import "./css/style.css"

function Blog() {
    let test = {
        subject: "Физика",
        grade: "10",
        heading: "Електромагнитни Вълни",
        description: "просто тест",
        school: "ППМГ Добри Чинтулов",
        num_questions: 20,
        average_score: 87.5,
        students: 15,
        time: 30,
        color1: "#358df1",
        color2: "#4797f2",
    }

    function toggleClass( ref, className){
        if (ref.classList.contains(className))
            ref.classList.remove(className);
        else
            ref.classList.add(className);
    }
    let trigger = React.createRef();
    let filter = React.createRef();
    let gallery = React.createRef();

    function OnTrigger(){
        //"filter-is-visible"
        toggleClass(trigger.current, "filter-is-visible");
        toggleClass(filter.current, "filter-is-visible");
        toggleClass(gallery.current, "filter-is-visible");
    }

    function OnCloseOpen(e){
        toggleClass(e.target, "closed")
        toggleClass(e.target.parentNode.children[1], "hide")
    }

    return (
        <div>
            <main className="cd-main-content">
                <section ref={gallery} className="cd-gallery">
                    <ul>
                        <Card test={test}/>
                        <Card test={test}/>
                        <Card test={test}/>
                        <Card test={test}/>
                    </ul>
                    <div className="cd-fail-message">Няма резултати</div>
                </section>
                <div ref={filter} className="cd-filter">
                    <form>
                        <div className="cd-filter-block">
                            <h4 onClick={OnCloseOpen}>Потърси</h4>

                            <div className="cd-filter-content">
                                <input type="search" placeholder="Потърси..."/>
                            </div>
                        </div>

                        <div className="cd-filter-block">
                            <h4 onClick={OnCloseOpen}>ПРедмет</h4>

                            <div className="cd-filter-content">
                                <div className="cd-select cd-filters">
                                    <select className="filter" name="selectThis" id="selectThis">
                                        <option value=".option1">Математика</option>
                                        <option value=".option2">БЕЛ</option>
                                        <option value=".option3">АЕ</option>
                                        <option value=".option4">Физика</option>
                                        <option value=".option4">Химия</option>
                                        <option value=".option4">Биология</option>
                                        <option value=".option4">История</option>
                                        <option value=".option4">География</option>
                                        <option value=".option4">Музика</option>
                                    </select>
                                </div>
                            </div>
                        </div>

                        <div className="cd-filter-block">
                            <h4 onClick={OnCloseOpen}>Клас</h4>

                            <div className="cd-filter-content">
                                <div className="cd-select cd-filters">
                                    <select className="filter" name="selectThis" id="selectThis">
                                        <option value=".option1">1 клас</option>
                                        <option value=".option2">2 клас</option>
                                        <option value=".option3">3 клас</option>
                                        <option value=".option4">4 клас</option>
                                        <option value=".option4">5 клас</option>
                                        <option value=".option4">6 клас</option>
                                        <option value=".option4">7 клас</option>
                                        <option value=".option4">8 клас</option>
                                        <option value=".option4">9 клас</option>
                                        <option value=".option4">10 клас</option>
                                        <option value=".option4">11 клас</option>
                                        <option value=".option4">12 клас</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                    </form>

                    <a href="#0" className="cd-close" onClick={OnTrigger}>X</a>
                </div>

                <a href="#0" ref={trigger} className="cd-filter-trigger" onClick={OnTrigger}>Филтри</a>
                </main>
        </div>
    );
}

export default Blog;
import React from 'react';
import "./css/home.css"
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import {faUsersLine} from "@fortawesome/free-solid-svg-icons"

function Home() {
    return (
        <div>
            <div className="splitter">
                <img src = {require("./img/test-img.png")} />
                <div>
                    <p style={{textAlign: "left", marginLeft: "50px"}}>IntelliTest е уеб платформа преназначена за учители с цел да олесни изготвянето и проверяването не тестове.
                    Предоставя възможност за автоматично генериране на въпроси от ключовите думи на даден текст.
                    Можеш да анализираш тестовете, с цел по-леснено оценяване.
                        Вече можеш да използваш автоматична проверка на въпросите с отворен отговор.</p>
                </div>
            </div>
            <section>
                <div className="row">
                    <h2>Why <span className="logoCol1">Intelli</span><span className="logoCol2">Test</span></h2>
                </div>
                <div className="row">
                    <div className="column">
                        <div className="card">
                            <div className="icon">
                                <img src={require("./img/question-feedback.png")} width={64} height={64}/>
                            </div>
                            <h3>Оценяване на въпроси с отворен отговор</h3>
                            <p>

                            </p>
                        </div>
                    </div>
                    <div className="column">
                        <div className="card">
                            <div className="icon">
                                <img src={require("./img/ai-chip.png")}/>
                            </div>
                            <h3>Генерирай въпроси от текст</h3>
                            <p>
                                IntelliTest използва вече тренираният модел T5
                                специално пригоден за генериране на въпроси от текст.
                                Намират се ключови думи, дати и имена и се задачат въпроси по тях.
                            </p>
                        </div>
                    </div>
                    <div className="column">
                        <div className="card">
                            <div className="icon">
                                <FontAwesomeIcon icon={faUsersLine} size="1x" color="black"/>
                            </div>
                            <h3>Анализиране на тестовете</h3>
                            <p>
                                Следи как се справят учниците според колко време е отнел тестът, колко е средният успех и кои са най-често срешаните грашки.
                            </p>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    );
}

export default Home;
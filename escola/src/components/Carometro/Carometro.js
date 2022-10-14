import React, { Component } from 'react';
import axios from 'axios';
import "./Carometro.css"



const urlAPI = 'http://localhost:5001/api/aluno';
const imgUrl = 'https://raw.githubusercontent.com/LiviaBrandao/projeto-escola-ds402/main/assets/';
const valorImg = [1, 2, 3, 4, 5, 6, 7, 8];


function getRandomInt(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

export default class Carometro extends Component {

    state = {
        dadosAlunos: []
    }

    componentDidMount() {
        axios(urlAPI).then(resp => {
            this.setState({ dadosAlunos: resp.data })
        })
    }

    render() {
        return (

            <div>
                <h1> Carômetro</h1>
                {this.state.dadosAlunos.map(
                    (aluno) =>
                        <div className="card" key={aluno.id}>

                            <img src={`${imgUrl}00${valorImg[getRandomInt(1,8)]}.png`}
                                alt={aluno.ra} />

                            <div className="container">
                                <h4 className='cardAluno'><b>{aluno.ra}</b></h4>
                                <p className='cardAluno'>{aluno.nome}</p>
                                <p className='cardAluno'> Curso: {aluno.codCurso}
                                </p>
                            </div>
                        </div>

                )
                }
            </div>
        )
    }

}
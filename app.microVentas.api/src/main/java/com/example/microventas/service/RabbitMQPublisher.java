package com.example.microventas.service;

import com.example.microventas.config.RabbitMQConfig;
import com.example.microventas.dto.VentaCreadaEvent;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.stereotype.Service;

@Service
public class RabbitMQPublisher {

    private final RabbitTemplate rabbitTemplate;

    public RabbitMQPublisher(RabbitTemplate rabbitTemplate) {
        this.rabbitTemplate = rabbitTemplate;
    }

    public void publicarVentaCreada(VentaCreadaEvent evento) {

        rabbitTemplate.convertAndSend(
                RabbitMQConfig.EXCHANGE_NAME,
                RabbitMQConfig.ROUTING_KEY,
                evento
        );

        System.out.println(
                "[RabbitMQ] Evento VentaCreada publicado: "
                        + evento.getVentaId()
        );
    }
}
package com.example.microventas.config;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.SerializationFeature;
import com.fasterxml.jackson.datatype.jsr310.JavaTimeModule;
import org.springframework.amqp.core.DirectExchange;
import org.springframework.amqp.core.Exchange;
import org.springframework.amqp.support.converter.Jackson2JsonMessageConverter;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class RabbitMQConfig {

    public static final String EXCHANGE_NAME = "ventas.exchange";
    public static final String ROUTING_KEY = "ventas.creada";

    @Bean
    public Exchange ventasExchange() {
        return new DirectExchange(
                EXCHANGE_NAME,
                true,
                false
        );
    }

    @Bean
    public Jackson2JsonMessageConverter jackson2JsonMessageConverter() {

        ObjectMapper objectMapper = new ObjectMapper();

        objectMapper.registerModule(new JavaTimeModule());

        objectMapper.disable(
                SerializationFeature.WRITE_DATES_AS_TIMESTAMPS
        );

        return new Jackson2JsonMessageConverter(objectMapper);
    }
}
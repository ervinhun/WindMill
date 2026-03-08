-- Enable UUID generation
CREATE
EXTENSION IF NOT EXISTS "uuid-ossp";

--------------------------------------------------
-- TURBINE TELEMETRY
--------------------------------------------------

CREATE TABLE turbine_telemetry
(
    id                  BIGSERIAL PRIMARY KEY,

    turbine_id          TEXT                     NOT NULL,
    timestamp           TIMESTAMP WITH TIME ZONE NOT NULL,

    wind_speed          DOUBLE PRECISION,
    wind_direction      DOUBLE PRECISION,
    ambient_temperature DOUBLE PRECISION,
    rotor_speed         DOUBLE PRECISION,
    power_output        DOUBLE PRECISION,
    nacelle_direction   DOUBLE PRECISION,
    blade_pitch         DOUBLE PRECISION,
    generator_temp      DOUBLE PRECISION,
    gearbox_temp        DOUBLE PRECISION,
    vibration           DOUBLE PRECISION,

    is_running          BOOLEAN                  NOT NULL,

    created_at          TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

--------------------------------------------------
-- ALERTS
--------------------------------------------------

CREATE TABLE turbine_alerts
(
    id         BIGSERIAL PRIMARY KEY,

    turbine_id TEXT                     NOT NULL,
    farm_id    UUID                     NOT NULL,

    timestamp  TIMESTAMP WITH TIME ZONE NOT NULL,

    severity   TEXT CHECK (severity IN ('info', 'warning', 'critical')),
    message    TEXT                     NOT NULL,

    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_alerts_turbine_time
    ON turbine_alerts (turbine_id, timestamp DESC);

--------------------------------------------------
-- ROLES
--------------------------------------------------

CREATE TABLE roles
(
    id          UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    name        TEXT UNIQUE NOT NULL,
    description TEXT
);

--------------------------------------------------
-- USERS
--------------------------------------------------

CREATE TABLE users
(
    id         UUID PRIMARY KEY         DEFAULT uuid_generate_v4(),

    name       TEXT        NOT NULL,
    username   TEXT UNIQUE NOT NULL,

    role_id    UUID        NOT NULL,

    is_deleted BOOLEAN                  DEFAULT FALSE,

    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_role
        FOREIGN KEY (role_id)
            REFERENCES roles (id)
);

CREATE INDEX idx_users_username
    ON users (username);

--------------------------------------------------
-- TURBINE COMMAND HISTORY
-- Tracks user modifications / commands
--------------------------------------------------

CREATE TABLE turbine_command_history
(
    id         BIGSERIAL PRIMARY KEY,

    turbine_id TEXT NOT NULL,
    user_id    UUID NOT NULL,

    action     TEXT NOT NULL,

    value      INTEGER,
    angle      DOUBLE PRECISION,
    reason     TEXT,

    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_command_user
        FOREIGN KEY (user_id)
            REFERENCES users (id)
);

CREATE INDEX idx_command_turbine_time
    ON turbine_command_history (turbine_id, created_at DESC);

--------------------------------------------------
-- OPTIONAL SETTINGS HISTORY (JSON BASED)
-- Useful if commands become dynamic
--------------------------------------------------

CREATE TABLE turbine_settings_history
(
    id         BIGSERIAL PRIMARY KEY,

    turbine_id TEXT NOT NULL,
    user_id    UUID NOT NULL,

    action     TEXT NOT NULL,
    settings   JSONB,

    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_settings_turbine
        FOREIGN KEY (turbine_id)
            REFERENCES turbines (id)
            ON DELETE CASCADE,

    CONSTRAINT fk_settings_user
        FOREIGN KEY (user_id)
            REFERENCES users (id)
);

CREATE INDEX idx_settings_turbine_time
    ON turbine_settings_history (turbine_id, created_at DESC);

--------------------------------------------------
-- TRIGGER TO AUTO UPDATE updated_at
--------------------------------------------------

CREATE
OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
   NEW.updated_at
= CURRENT_TIMESTAMP;
RETURN NEW;
END;
$$
language 'plpgsql';

CREATE TRIGGER update_users_updated_at
    BEFORE UPDATE
    ON users
    FOR EACH ROW
    EXECUTE FUNCTION update_updated_at_column();